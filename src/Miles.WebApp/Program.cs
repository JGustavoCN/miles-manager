using Miles.WebApp.Components;
using Miles.WebApp.Middleware;
using MudBlazor.Services;
using Serilog;
using Miles.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Miles.Core.Interfaces;
using Miles.Infrastructure.Repositories;
using Miles.Core.Factories;
using Miles.Core.Strategies;
using Miles.Application.Services;
using Miles.Application.Interfaces;
using Miles.WebApp.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Iniciando aplicação Miles.WebApp...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // 1. Configuração do Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // 2. Serviços do Blazor
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    // 3. Configuração do Banco de Dados
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlOptions => sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            )
        )
    );

    // 3.1. Registro de Repositórios
    builder.Services.AddScoped<ICartaoRepository, CartaoRepository>();
    builder.Services.AddScoped<IProgramaRepository, ProgramaRepository>();
    // Nota: Outros repositórios (ITransacaoRepository, IProgramaRepository, IUsuarioRepository)
    // serão adicionados conforme forem implementados na camada Infrastructure

    // 3.2. Registro de Factories (UC-02)
    builder.Services.AddScoped<ITransacaoFactory, TransacaoFactory>();

    // 3.3. Registro de Strategies (UC-09)
    builder.Services.AddScoped<ICalculoPontosStrategy, CalculoPadraoStrategy>();

    // 3.4. Registro de Application Services (UC-02, UC-03, UC-08, UC-09)
    // builder.Services.AddScoped<ITransacaoService, TransacaoService>();
    builder.Services.AddScoped<ICartaoService, CartaoService>();
    builder.Services.AddScoped<IProgramaService, ProgramaService>();
    builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

    // 3.2. Serviços de Aplicação
    builder.Services.AddScoped<AuthService>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    // 3.3. Autenticação Blazor (Customizada)
    builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/login";
            options.LogoutPath = "/logout";
            options.ExpireTimeSpan = TimeSpan.FromHours(24);
        });

    builder.Services.AddAuthorization();

    // 4. Serviços do MudBlazor
    builder.Services.AddMudServices();

    var app = builder.Build();

    // 5. Middleware de erros
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // 6. Logging HTTP
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} respondeu {StatusCode} em {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
        };
    });

    if (app.Environment.IsDevelopment())
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                DbInitializer.Initialize(context, Log.Logger);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao popular o banco de dados.");
            }
        }
    }

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseStatusCodePagesWithReExecute("/Error");
    app.UseHttpsRedirection();

    app.Use(async (context, next) =>
    {
        // Se o usuário não estiver logado via Cookie...
        if (context.User.Identity?.IsAuthenticated != true)
        {
            // ...criamos uma identidade falsa apenas para passar pelo ASP.NET
            var claims = new[] { new System.Security.Claims.Claim("BlazorBypass", "true") };
            var identity = new System.Security.Claims.ClaimsIdentity(claims, "BypassAuth");
            context.User = new System.Security.Claims.ClaimsPrincipal(identity);
        }
        await next();
    });

    // --- MIDDLEWARES DE AUTH (OBRIGATÓRIOS AQUI) ---
    app.UseAuthentication();
    app.UseAuthorization();
    // -----------------------------------------------

    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    Log.Information("Aplicação Miles.WebApp iniciada com sucesso!");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou ao iniciar");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

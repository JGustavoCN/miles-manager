using Miles.WebApp.Components;
using Miles.WebApp.Middleware;
using MudBlazor.Services;
using Serilog;
using Miles.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Miles.Core.Interfaces;
using Miles.Infrastructure.Repositories;
using Miles.Application.Services;
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

    // 1. Configuração do Serilog para substituir o logger padrão
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // 2. Serviços do Blazor
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    // 3. Configuração do Banco de Dados (Vindo da MAIN)
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

    // 3.1. Registro de Repositórios (DI)
    builder.Services.AddScoped<ICartaoRepository, CartaoRepository>();
    builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

    // 3.2. Registro de Serviços da Aplicação (UC-01)
    builder.Services.AddScoped<AuthService>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    // 3.3. Configuração de Autenticação e Autorização Blazor (UC-01)
    builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
    builder.Services.AddCascadingAuthenticationState();

    // 3.4. Configuração de Autenticação ASP.NET Core
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/login";
            options.LogoutPath = "/logout";
            options.AccessDeniedPath = "/access-denied";
            options.ExpireTimeSpan = TimeSpan.FromHours(24);
            options.SlidingExpiration = true;
        });
    builder.Services.AddAuthorization();

    // 4. Serviços do MudBlazor
    builder.Services.AddMudServices();

    var app = builder.Build();

    // 5. Middleware de tratamento global de exceções (Vindo da FEAT)
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // 6. Logging de requisições HTTP (Request Logging do Serilog)
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
                var logger = services.GetRequiredService<ILogger<Program>>();
                Log.Error(ex, "Erro ao popular o banco de dados com Seed Data.");
            }
        }
    }
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseStatusCodePagesWithReExecute("/Error");
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    Log.Information("Aplicação Miles.WebApp iniciada com sucesso!");
    app.Run();
}
catch (Exception ex)
{
    // Captura erros fatais de inicialização (ex: falha na conexão com banco ao iniciar)
    Log.Fatal(ex, "A aplicação falhou ao iniciar");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

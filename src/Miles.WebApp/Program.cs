using Miles.WebApp.Components;
using Miles.WebApp.Middleware;
using MudBlazor.Services;
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
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Microsoft.AspNetCore.HttpOverrides; // 1. Namespace adicionado

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
    builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
    builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

    // 3.2. Registro de Factories (UC-02)
    builder.Services.AddScoped<ITransacaoFactory, TransacaoFactory>();

    // 3.3. Registro de Strategies (UC-09)
    builder.Services.AddScoped<ICalculoPontosStrategy, CalculoPadraoStrategy>();

    // 3.4. Registro de Application Services
    builder.Services.AddScoped<ICartaoService, CartaoService>();
    builder.Services.AddScoped<IProgramaService, ProgramaService>();
    builder.Services.AddScoped<ITransacaoService, TransacaoService>();
    builder.Services.AddScoped<IDashboardFacade, DashboardFacade>();
    builder.Services.AddScoped<IUsuarioService, UsuarioService>();

    // Serviços de Autenticação
    builder.Services.AddScoped<AuthService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
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
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<AppDbContext>();

    // 5. Configuração de Forwarded Headers (CRÍTICO PARA AZURE/LINUX)
    // Isso garante que o app saiba que a requisição original foi HTTPS
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

        // Em nuvem (Azure/AWS), o IP do proxy pode mudar, então limpamos as redes conhecidas/proxies
        // para confiar nos headers vindos do Load Balancer interno.
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });

    var app = builder.Build();

    // 6. Aplicar Forwarded Headers (Deve ser o PRIMEIRO Middleware)
    app.UseForwardedHeaders();

    // 7. Middleware de erros
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // 8. Logging HTTP
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} respondeu {StatusCode} em {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
        };
    });

    // Configurações de Ambiente
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
    else // !IsDevelopment (Produção/Staging)
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);

        // HSTS instrui o navegador a sempre usar HTTPS
        app.UseHsts();

        // Redirecionamento HTTPS condicional
        // Só redireciona em Produção e funciona corretamente graças ao UseForwardedHeaders acima
        app.UseHttpsRedirection();
    }

    app.UseStatusCodePagesWithReExecute("/Error");

    // app.UseHttpsRedirection(); <--- REMOVIDO DAQUI (Movido para dentro do else acima)

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

    // --- MIDDLEWARES DE AUTH ---
    app.UseAuthentication();
    app.UseAuthorization();
    // ---------------------------

    app.UseAntiforgery();

    app.MapStaticAssets();

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = e.Value.Duration.ToString()
                })
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    });

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    Log.Information("Aplicação Miles.WebApp iniciada com sucesso!");
    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "A aplicação falhou ao iniciar");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

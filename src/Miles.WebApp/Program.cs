using Miles.WebApp.Components;
using Miles.WebApp.Middleware;
using MudBlazor.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Iniciando aplicação Miles.WebApp...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Substituir o sistema de logging padrão do ASP.NET Core pelo Serilog
    // Lê a configuração completa do appsettings.json
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    // TODO: Configurar Entity Framework Core quando necessário
    // using Miles.Infrastructure.Data;
    // using Microsoft.EntityFrameworkCore;
    // builder.Services.AddDbContext<AppDbContext>(options =>
    //     options.UseSqlServer(
    //         builder.Configuration.GetConnectionString("DefaultConnection"),
    //         sqlOptions => sqlOptions.EnableRetryOnFailure(
    //             maxRetryCount: 5,
    //             maxRetryDelay: TimeSpan.FromSeconds(30),
    //             errorNumbersToAdd: null
    //         )
    //     )
    // );

    // Registrar serviços do MudBlazor
    builder.Services.AddMudServices();

    var app = builder.Build();

    // Middleware de tratamento global de exceções
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Adicionar logging de requisições HTTP (Request Logging do Serilog)
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} respondeu {StatusCode} em {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
        };
    });

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        // Em produção, usar página de erro customizada
        // O ExceptionHandlingMiddleware já trata exceções, mas mantemos como fallback
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseStatusCodePagesWithReExecute("/Error");
    app.UseHttpsRedirection();

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


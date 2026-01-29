using System.Net;
using System.Text.Json;

namespace Miles.WebApp.Middleware;

/// <summary>
/// Middleware global para captura e tratamento centralizado de exceções.
/// 
/// Justificativa Técnica:
/// - Captura exceções não tratadas em qualquer ponto do pipeline
/// - Loga detalhes completos (Stack Trace) usando Serilog para diagnóstico
/// - Retorna respostas JSON padronizadas para o cliente (evita expor detalhes internos)
/// - Diferencia ambiente de desenvolvimento (retorna detalhes) de produção (retorna mensagem genérica)
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Log detalhado da exceção com Stack Trace completo
        _logger.LogError(exception,
            "Exceção não tratada capturada: {ExceptionType} - {Message} | Path: {Path} | Method: {Method}",
            exception.GetType().Name,
            exception.Message,
            context.Request.Path,
            context.Request.Method);

        // Configuração da resposta HTTP
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Resposta JSON padronizada
        var response = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = "Ocorreu um erro interno no servidor.",
            // Em desenvolvimento, expor detalhes; em produção, ocultar
            Detail = _environment.IsDevelopment()
                ? $"{exception.GetType().Name}: {exception.Message}"
                : null,
            StackTrace = _environment.IsDevelopment()
                ? exception.StackTrace
                : null
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}

/// <summary>
/// Modelo de resposta de erro padronizado
/// </summary>
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Detail { get; set; }
    public string? StackTrace { get; set; }
}

# 🚀 Guia Rápido - Logging e Exception Handling

## ✅ O que foi implementado

### 1. Pacotes Instalados
- ✅ `Serilog.AspNetCore` (v10.0.0)
- ✅ `Serilog.Sinks.File` (v7.0.0)

---

## 🎯 Recursos Implementados

### Serilog Configuration
- **Console Sink**: Logs coloridos no terminal
- **File Sink**: Logs em `Logs/miles-log-{Date}.txt`
  - Rotação diária
  - Retenção de 30 dias
  - Limite de 10MB por arquivo

### Exception Handling Middleware
- Captura todas as exceções não tratadas
- Loga detalhes completos (Stack Trace)
- Retorna JSON padronizado:
  ```json
  {
    "statusCode": 500,
    "message": "Ocorreu um erro interno no servidor.",
    "detail": "Tipo: mensagem (somente em dev)",
    "stackTrace": "..." (somente em dev)
  }
  ```

### Request Logging
- Loga automaticamente todas as requisições HTTP
- Formato: `HTTP {Method} {Path} respondeu {StatusCode} em {Elapsed} ms`

---

## 🧪 Como Testar

### 1. Executar a aplicação
```bash
dotnet run --project src\Miles.WebApp\Miles.WebApp.csproj
```

### 2. Acessar página de teste
```
https://localhost:5001/test-logging
```

A página oferece botões para:
- Testar diferentes níveis de log (Trace, Debug, Info, Warning, Error, Critical)
- Forçar exceções para validar o middleware
- Criar logs estruturados

### 3. Verificar os logs

**No Console:**
```
[2026-01-29 14:32:15.234 -03:00] [INF] Iniciando aplicação Miles.WebApp...
[2026-01-29 14:32:16.123 -03:00] [INF] HTTP GET /test-logging respondeu 200 em 45.67 ms
```

**No Arquivo (`Logs/miles-log-20260129.txt`):**
```
[2026-01-29 14:32:15.234 -03:00] [INF] [Miles.WebApp.Program] Iniciando aplicação Miles.WebApp...
[2026-01-29 14:32:16.123 -03:00] [ERR] [Miles.WebApp.Middleware.ExceptionHandlingMiddleware] Exceção não tratada...
System.InvalidOperationException: Teste do ExceptionHandlingMiddleware
   at Miles.WebApp.Components.Pages.TestLogging.ThrowException() in ...
```

---

## 📊 Níveis de Log Configurados

| Namespace | Nível (Produção) | Nível (Dev) |
|-----------|------------------|-------------|
| Default | Information | Debug |
| Microsoft | Warning | Information |
| Microsoft.AspNetCore | Warning | Warning |
| Microsoft.EntityFrameworkCore | Information | Information |
| System | Warning | Information |

---

## 🔥 Uso no Código

### Injetar Logger
```csharp
public class MyService
{
    private readonly ILogger<MyService> _logger;

    public MyService(ILogger<MyService> logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.LogInformation("Executando operação...");
        
        try
        {
            // código
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar operação");
            throw;
        }
    }
}
```

### Logs Estruturados
```csharp
_logger.LogInformation(
    "Usuário {UserId} acessou recurso {ResourceId} em {Timestamp}",
    userId,
    resourceId,
    DateTime.UtcNow
);
```

---

## 💡 Por que Serilog?

1. **Logs Estruturados** - Propriedades semânticas para análise avançada
2. **Sinks Flexíveis** - Console, File, Seq, Azure, Elasticsearch...
3. **Performance** - Logging assíncrono otimizado
4. **Configuração Externa** - Ajustar níveis sem recompilar
5. **Enriquecimento** - MachineName, ThreadId, RequestId automáticos
6. **Maturidade** - Amplamente adotado na comunidade .NET

---

**Status**: ✅ Implementado e testado  
**Build**: ✅ Sucesso  
**Pronto para uso em**: Desenvolvimento e Produção

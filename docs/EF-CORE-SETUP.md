# 🔧 Configuração do Entity Framework Core - Miles Manager

## 📋 Sumário Executivo

Este documento detalha a implementação da camada de acesso a dados utilizando **Entity Framework Core 8.0** com **SQL Server** no projeto Miles Manager. A configuração segue os princípios da **Clean Architecture**, isolando as dependências de infraestrutura na camada `Miles.Infrastructure`.

---

## 🏗️ Arquitetura Implementada

### Separação de Responsabilidades

```bash
┌─────────────────────────────────────────────────────────────┐
│                    Miles.WebApp (UI)                        │
│  • Program.cs → Injeção de Dependência                     │
│  • appsettings.json → Connection String                    │
└──────────────────────────┬──────────────────────────────────┘
                           │ Referencia
                           ↓
┌─────────────────────────────────────────────────────────────┐
│              Miles.Infrastructure (Data Access)             │
│  • AppDbContext → Gerencia conexão com BD                  │
│  • Migrations → Versionamento do Schema (Futuro)           │
│  • Repositories → Padrão Repository (Futuro)               │
└──────────────────────────┬──────────────────────────────────┘
                           │ Referencia
                           ↓
┌─────────────────────────────────────────────────────────────┐
│                 Miles.Core (Domain)                         │
│  • Entidades de Domínio (Futuro)                           │
│  • Interfaces de Repositórios (Futuro)                     │
└─────────────────────────────────────────────────────────────┘
```

### Por que essa estrutura?

- **Miles.Core:** Não possui dependências externas (nem EF Core). Apenas lógica de negócio pura.
- **Miles.Infrastructure:** Contém todas as dependências de tecnologia (EF Core, SQL Server).
- **Miles.WebApp:** Apenas consome serviços via Injeção de Dependência, sem conhecer detalhes de implementação.

---

## 📦 Pacotes NuGet Instalados

| Pacote                                    | Versão | Função                                          |
| ----------------------------------------- | ------ | ----------------------------------------------- |
| `Microsoft.EntityFrameworkCore.SqlServer` | 8.0.11 | Provider para SQL Server                        |
| `Microsoft.EntityFrameworkCore.Tools`     | 8.0.11 | Ferramentas CLI para Migrations (`dotnet ef`)   |
| `Microsoft.EntityFrameworkCore.Design`    | 8.0.11 | Suporte para Design-Time (Scaffold, Migrations) |

### Comando de Instalação

```bash
cd src/Miles.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.11
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.11
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.11
```

---

## 🗄️ Configuração do AppDbContext

### Implementação

O [`AppDbContext`](../src/Miles.Infrastructure/Data/AppDbContext.cs) é a classe central que gerencia a comunicação com o banco de dados:

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configurações de entidades serão aplicadas aqui
    }
}
```

### Características

- ✅ **Construtor com Injeção de Dependência:** Recebe `DbContextOptions` via DI, permitindo configuração centralizada.
- ✅ **OnModelCreating:** Método reservado para aplicação de configurações Fluent API (mapeamento de entidades).
- ✅ **Preparado para DbSets:** À medida que as entidades forem criadas, serão adicionadas como propriedades (ex: `public DbSet<Usuario> Usuarios { get; set; }`).

---

## 🔌 Connection String

### Configuração no appsettings.json

A string de conexão foi adicionada ao [`appsettings.json`](../src/Miles.WebApp/appsettings.json) do projeto WebApp:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=MilesDb;User Id=sa;Password=Miles@Manager2026!;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Parâmetros Explicados

| Parâmetro                  | Valor                | Justificativa                                                    |
| -------------------------- | -------------------- | ---------------------------------------------------------------- |
| `Server`                   | `localhost,1433`     | Instância SQL Server rodando no Docker (porta padrão)            |
| `Database`                 | `MilesDb`            | Nome do banco de dados conforme especificação do projeto         |
| `User Id`                  | `sa`                 | Conta de administrador do SQL Server                             |
| `Password`                 | `Miles@Manager2026!` | Senha configurada no [docker-compose.yml](../docker-compose.yml) |
| `TrustServerCertificate`   | `True`               | Necessário para conexões locais sem certificado SSL válido       |
| `MultipleActiveResultSets` | `True`               | Permite múltiplas queries simultâneas na mesma conexão           |

⚠️ **Segurança:** Esta senha é **apenas para desenvolvimento local**. Nunca deve ser usada em produção.

---

## ⚙️ Configuração da Injeção de Dependência

### Implementação no Program.cs

No [`Program.cs`](../src/Miles.WebApp/Program.cs), o `AppDbContext` foi registrado no container de DI:

```csharp
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
```

### Recursos Habilitados

#### 1. Retry Policy (Resiliência)

**Por que é importante?**  
Em ambientes de produção, falhas temporárias de rede ou do SQL Server podem ocorrer. A política de retry garante que operações falhas sejam automaticamente reexecutadas.

**Configuração Aplicada:**

- **maxRetryCount:** 5 tentativas
- **maxRetryDelay:** Até 30 segundos de espera entre tentativas
- **Algoritmo:** Exponential Backoff (aumenta o tempo entre tentativas progressivamente)

**Exemplo de Comportamento:**

```bash
Tentativa 1: Falha → Aguarda 1s
Tentativa 2: Falha → Aguarda 2s
Tentativa 3: Falha → Aguarda 4s
Tentativa 4: Falha → Aguarda 8s
Tentativa 5: Sucesso ✅
```

#### 2. Connection Pooling (Padrão do EF Core)

O EF Core gerencia automaticamente um pool de conexões, reutilizando-as entre requisições para melhor performance.

---

## 📝 Logging e Monitoramento

### Configuração de Logs do EF Core

No [`appsettings.json`](../src/Miles.WebApp/appsettings.json), foi habilitado o log de comandos SQL:

```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning",
    "Microsoft.EntityFrameworkCore.Database.Command": "Information"
  }
}
```

**O que será logado:**

- Comandos SQL executados (SELECT, INSERT, UPDATE, DELETE)
- Tempo de execução de cada query
- Parâmetros passados para queries parametrizadas

**Exemplo de Log:**

```bash
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (23ms) [Parameters=[@p0='?' (Size = 100)], CommandType='Text', CommandTimeout='30']
      INSERT INTO [Usuarios] ([Nome]) VALUES (@p0);
```

---

## 🔒 Boas Práticas Implementadas

### 1. ✅ Separação de Concerns (Clean Architecture)

- **Core:** Não conhece EF Core (apenas interfaces)
- **Infrastructure:** Implementa detalhes técnicos
- **WebApp:** Apenas consome serviços

### 2. ✅ Configuração Externa (12-Factor App)

Connection String no `appsettings.json`, não hardcoded no código.

### 3. ✅ Resiliência

Retry Policy habilitada para lidar com falhas temporárias.

### 4. ✅ Observabilidade

Logs de SQL habilitados para debugging.

### 5. ✅ Versionamento do Schema

Migrations serão usadas para controlar evolução do banco.

---

## 📚 Referências Técnicas

- [Entity Framework Core Documentation](https://learn.microsoft.com/ef/core/)
- [Connection Strings - SQL Server](https://learn.microsoft.com/ef/core/miscellaneous/connection-strings)
- [Connection Resiliency](https://learn.microsoft.com/ef/core/miscellaneous/connection-resiliency)
- [Migrations Overview](https://learn.microsoft.com/ef/core/managing-schemas/migrations/)
- [Clean Architecture - Jason Taylor](https://github.com/jasontaylordev/CleanArchitecture)

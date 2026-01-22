# 🗄️ Configuração do Banco de Dados - Miles Manager

## Pré-requisitos

- Docker Desktop instalado e rodando
- Porta 1433 disponível (verifique se não há outra instância do SQL Server rodando)

## 🚀 Como Subir o Banco de Dados

### 1. Subir o Container

Na raiz do projeto, execute:

```bash
docker-compose up -d
```

### 2. Verificar se está Rodando

```bash
docker ps
```

Você deve ver o container `miles-manager-sqlserver` com status "healthy".

### 3. Ver os Logs (opcional)

```bash
docker-compose logs -f sqlserver
```

## 🔌 Connection String

### Para appsettings.json (Entity Framework Core)

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=MilesManagerDb;User Id=sa;Password=Miles@Manager2026!;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

### Para DBeaver

- **Host:** localhost
- **Port:** 1433
- **Database:** MilesManagerDb (ou deixe vazio inicialmente)
- **Username:** sa
- **Password:** Miles@Manager2026!
- **SSL:** Desabilitado ou Trust Server Certificate

### Para Azure Data Studio (Recomendado)

- **Server:** localhost,1433
- **Authentication type:** SQL Login
- **User name:** sa
- **Password:** Miles@Manager2026!
- **Encrypt:** Optional
- **Trust server certificate:** True

### Para SQL Server Management Studio (SSMS)

- **Server name:** localhost,1433
- **Authentication:** SQL Server Authentication
- **Login:** sa
- **Password:** Miles@Manager2026!
- **Encryption:** Optional
- **Trust Server Certificate:** Yes

## 🧪 Testar a Conexão

### Usando Docker Exec (Mais Rápido)

```bash
docker exec -it miles-manager-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Miles@Manager2026!" -Q "SELECT @@VERSION"
```

### Usando Azure Data Studio (Recomendado)

1. Baixe: <https://aka.ms/azuredatastudio>
2. Clique em "New Connection"
3. Preencha os dados acima
4. Clique em "Connect"
5. Execute: `SELECT @@VERSION`

### Usando DBeaver (Alternativa)

1. New Database Connection → SQL Server
2. Preencha os dados acima
3. Test Connection → OK

## 🛑 Comandos Úteis

### Parar o Banco de Dados

```bash
docker-compose down
```

### Reiniciar o Banco

```bash
docker-compose restart sqlserver
```

### Ver Logs em Tempo Real

```bash
docker-compose logs -f sqlserver
```

### Remover Banco e Dados (⚠️ CUIDADO!)

```bash
docker-compose down -v
```

## 📝 Notas Importantes

### Segurança

- A senha `Miles@Manager2026!` é para **desenvolvimento local apenas**
- **NUNCA** commite senhas reais no repositório
- Para produção, use Azure Key Vault ou variáveis de ambiente
- O arquivo `.env` já está no `.gitignore`

### Persistência de Dados

- Os dados são armazenados no volume Docker `sqlserver_data`
- Mesmo parando o container (`docker-compose down`), os dados são mantidos
- Use `docker-compose down -v` apenas se quiser apagar TODOS os dados

### Requisitos de Sistema

- **Memória:** SQL Server precisa de ~2GB de RAM
- **Disco:** ~10GB recomendado para o volume
- **CPU:** 2+ cores recomendado

## 🔧 Troubleshooting

### Porta 1433 já em uso

**Windows - Parar o SQL Server local:**

```powershell
net stop MSSQLSERVER
# ou
Get-Service -Name MSSQL* | Stop-Service
```

**Ou altere a porta no docker-compose.yml:**

```yaml
ports:
  - '1434:1433' # Usar porta 1434 externamente
```

E ajuste a connection string para `Server=localhost,1434;...`

### Container não inicia

```bash
# Ver logs detalhados
docker-compose logs sqlserver

# Verificar memória disponível
docker stats

# Verificar se o Docker Desktop está rodando
docker info
```

### Erro "SA password does not meet complexity requirements"

- A senha deve ter pelo menos 8 caracteres
- Incluir maiúsculas, minúsculas, números e caracteres especiais
- A senha atual `Miles@Manager2026!` já atende todos os requisitos

### Erro de conexão "SSL Provider, error: 31"

- Adicione `TrustServerCertificate=True` na connection string
- Ou desabilite a encriptação na ferramenta de conexão

### Container fica reiniciando

```bash
# Ver logs
docker logs miles-manager-sqlserver

# Verificar memória (SQL precisa de ~2GB)
docker stats

# Verificar se a porta está disponível
netstat -an | findstr 1433
```

## 📚 Links Úteis

- [SQL Server Docker Hub](https://hub.docker.com/_/microsoft-mssql-server)
- [Entity Framework Core com SQL Server](https://learn.microsoft.com/ef/core/providers/sql-server/)
- [Azure Data Studio](https://aka.ms/azuredatastudio)
- [DBeaver](https://dbeaver.io/download/)

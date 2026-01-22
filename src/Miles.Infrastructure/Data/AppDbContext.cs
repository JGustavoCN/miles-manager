using Microsoft.EntityFrameworkCore;

namespace Miles.Infrastructure.Data;

/// <summary>
/// Contexto principal do Entity Framework Core para o Miles Manager.
/// Responsável pela comunicação com o banco de dados SQL Server.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Construtor padrão que recebe as opções de configuração do contexto.
    /// </summary>
    /// <param name="options">Opções de configuração do DbContext (Connection String, Provider, etc.)</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Configurações adicionais do modelo de dados.
    /// Aqui serão aplicadas as configurações de entidades via Fluent API quando necessário.
    /// </summary>
    /// <param name="modelBuilder">Construtor do modelo de dados</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TODO: Aplicar configurações de entidades quando forem criadas
        // Exemplo: modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
    }

    // TODO: Adicionar DbSets conforme as entidades forem criadas
    // Exemplo: public DbSet<Usuario> Usuarios { get; set; }
}

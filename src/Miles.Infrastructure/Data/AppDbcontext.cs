using Microsoft.EntityFrameworkCore;
using Miles.Core.Entities;

namespace Miles.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<ProgramaFidelidade> ProgramasFidelidade => Set<ProgramaFidelidade>();
    public DbSet<Cartao> Cartoes => Set<Cartao>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas as configurações de mapeamento automaticamente
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

    }

}

using Microsoft.EntityFrameworkCore;
using Miles.Core.Entities;

namespace Miles.Infrastructure.Data;

/// Contexto principal do Entity Framework Core para o Miles Manager.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<ProgramaFidelidade> Programas { get; set; }
    public DbSet<Cartao> Cartoes { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurarUsuario(modelBuilder);
        ConfigurarProgramaFidelidade(modelBuilder);
        ConfigurarCartao(modelBuilder);
        ConfigurarTransacao(modelBuilder);
    }

    private void ConfigurarUsuario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);

            // Email deve ser único
            entity.HasIndex(u => u.Email).IsUnique();

            // Relacionamento com Cartoes
            entity.HasMany(u => u.Cartoes)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Programas
            entity.HasMany(u => u.Programas)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigurarProgramaFidelidade(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProgramaFidelidade>(entity =>
        {
            entity.HasKey(p => p.Id);

            // Relacionamento com Cartoes
            entity.HasMany(p => p.Cartoes)
                .WithOne(c => c.Programa)
                .HasForeignKey(c => c.ProgramaId)
                .OnDelete(DeleteBehavior.Restrict); // Não deleta em cascata
        });
    }

    private void ConfigurarCartao(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cartao>(entity =>
        {
            entity.HasKey(c => c.Id);

            // Precisão para valores monetários
            entity.Property(c => c.Limite)
                .HasPrecision(18, 2);

            entity.Property(c => c.FatorConversao)
                .HasPrecision(5, 2);

            // Relacionamento com Transacoes
            entity.HasMany(c => c.Transacoes)
                .WithOne(t => t.Cartao)
                .HasForeignKey(t => t.CartaoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigurarTransacao(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.HasKey(t => t.Id);

            // Precisão para valores monetários
            entity.Property(t => t.Valor)
                .HasPrecision(18, 2);

            // Cotação do dólar com 4 casas decimais (ex: 5.1234)
            entity.Property(t => t.CotacaoDolar)
                .HasPrecision(10, 4);
        });
    }
}

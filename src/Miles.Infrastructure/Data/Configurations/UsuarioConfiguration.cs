using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miles.Core.Entities;

namespace Miles.Infrastructure.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        // Tabela
        builder.ToTable("Usuarios");

        // Chave Primária
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        // Propriedades
        builder.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("VARCHAR(100)");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("VARCHAR(150)");

        // Índice Único para Email (Previne duplicação)
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Usuarios_Email");

        builder.Property(u => u.SenhaHash)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType("VARCHAR(255)");

        // Relacionamentos (1:N)
        builder.HasMany(u => u.Cartoes)
            .WithOne(c => c.Usuario)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade); // Se usuário for excluído, deleta cartões

        builder.HasMany(u => u.Programas)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miles.Core.Entities;

namespace Miles.Infrastructure.Data.Configurations;

public class ProgramaFidelidadeConfiguration : IEntityTypeConfiguration<ProgramaFidelidade>
{
    public void Configure(EntityTypeBuilder<ProgramaFidelidade> builder)
    {
        // Tabela
        builder.ToTable("ProgramasFidelidade");

        // Chave Primária
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        // Propriedades
        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("VARCHAR(100)");

        builder.Property(p => p.Banco)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("VARCHAR(100)");

        // Foreign Key (Usuario)
        builder.Property(p => p.UsuarioId)
            .IsRequired();

        // Relacionamento reverso já configurado em UsuarioConfiguration
    }
}

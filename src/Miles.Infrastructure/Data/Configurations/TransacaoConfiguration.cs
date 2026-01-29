using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miles.Core.Entities;

namespace Miles.Infrastructure.Data.Configurations;

public class TransacaoConfiguration : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        // Tabela
        builder.ToTable("Transacoes");

        // Chave Primária
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        // Propriedades
        builder.Property(t => t.Data)
            .IsRequired()
            .HasColumnType("DATETIME2");

        // Índice para otimizar buscas por data
        builder.HasIndex(t => t.Data)
            .HasDatabaseName("IX_Transacoes_Data");

        // Campos Monetários (Precisão 18,2 para valores em Reais)
        builder.Property(t => t.Valor)
            .IsRequired()
            .HasColumnType("DECIMAL(18,2)");

        builder.Property(t => t.CotacaoDolar)
            .IsRequired()
            .HasColumnType("DECIMAL(10,4)"); // Precisão maior para cotação

        builder.Property(t => t.Descricao)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("VARCHAR(200)");

        builder.Property(t => t.Categoria)
            .HasMaxLength(50)
            .HasColumnType("VARCHAR(50)");

        // Pontos (Snapshot calculado)
        builder.Property(t => t.PontosEstimados)
            .IsRequired()
            .HasColumnType("INT");

        // Foreign Key
        builder.Property(t => t.CartaoId)
            .IsRequired();

        // Relacionamento reverso já configurado em CartaoConfiguration
    }
}

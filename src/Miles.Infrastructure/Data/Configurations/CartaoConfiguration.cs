using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miles.Core.Entities;

namespace Miles.Infrastructure.Data.Configurations;

public class CartaoConfiguration : IEntityTypeConfiguration<Cartao>
{
    public void Configure(EntityTypeBuilder<Cartao> builder)
    {
        // Tabela
        builder.ToTable("Cartoes");

        // Chave Primária
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        // Propriedades
        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("VARCHAR(100)");

        builder.Property(c => c.Bandeira)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("VARCHAR(50)");

        // Campos Monetários (Precisão 18,2)
        builder.Property(c => c.Limite)
            .IsRequired()
            .HasColumnType("DECIMAL(18,2)");

        builder.Property(c => c.DiaVencimento)
            .IsRequired()
            .HasColumnType("INT");

        // Fator de Conversão (Precisão 10,4 para suportar fatores como 1.5000)
        builder.Property(c => c.FatorConversao)
            .IsRequired()
            .HasColumnType("DECIMAL(10,4)")
            .HasDefaultValue(1.0m);

        // Foreign Keys
        builder.Property(c => c.UsuarioId)
            .IsRequired();

        builder.Property(c => c.ProgramaId)
            .IsRequired();

        // Relacionamentos
        builder.HasOne(c => c.Programa)
            .WithMany(p => p.Cartoes)
            .HasForeignKey(c => c.ProgramaId)
            .OnDelete(DeleteBehavior.Restrict); // Não permite deletar programa se houver cartões

        builder.HasMany(c => c.Transacoes)
            .WithOne(t => t.Cartao)
            .HasForeignKey(t => t.CartaoId)
            .OnDelete(DeleteBehavior.Cascade); // Deleta transações ao deletar cartão
    }
}

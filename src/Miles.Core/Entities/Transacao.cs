using System.ComponentModel.DataAnnotations;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;

namespace Miles.Core.Entities;

/// Representa uma transação financeira que gera pontos (RF-005)
/// Funciona como snapshot auditável (cotação e pontos são congelados)
public class Transacao
{
    public int Id { get; set; }

    [Required]
    public DateTime Data { get; set; } = DateTime.Now;

    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "Descrição é obrigatória")]
    [MaxLength(200)]
    public string Descricao { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Categoria { get; set; } = string.Empty;

    [Range(0.01, 100.0)]
    public decimal CotacaoDolar { get; set; }

    public int PontosEstimados { get; set; }

    // Foreign Key
    public int CartaoId { get; set; }
    public virtual Cartao Cartao { get; set; } = null!;

    /// Valida valores financeiros antes de cálculos (RF-008)
    private void ValidarValor()
    {
        if (Valor <= 0)
        {
            throw new ValorInvalidoException("Valor da transação deve ser maior que zero");
        }

        if (CotacaoDolar <= 0)
        {
            throw new ValorInvalidoException("Cotação do dólar deve ser maior que zero");
        }
    }

    /// Calcula pontos usando Strategy Pattern (RF-006)
    /// Fórmula: (Valor em R$ / Cotação USD) * Fator do Cartão
    public void CalcularPontos(ICalculoPontosStrategy strategy)
    {
        ValidarValor();
        PontosEstimados = strategy.Calcular(Valor, CotacaoDolar, Cartao.FatorConversao);
    }

    /// Valida todos os campos obrigatórios (RF-008)
    public void Validar()
    {
        ValidarValor();

        if (string.IsNullOrWhiteSpace(Descricao))
        {
            throw new ValorInvalidoException("Descrição da transação é obrigatória");
        }

        if (Data > DateTime.Now)
        {
            throw new ValorInvalidoException("Data da transação não pode ser futura");
        }
    }
}

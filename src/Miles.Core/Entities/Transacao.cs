using System.ComponentModel.DataAnnotations;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;

namespace Miles.Core.Entities;

/// Representa uma transação financeira que gera pontos (RF-005).
/// Funciona como snapshot auditável (cotação e pontos são congelados).
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

    /// <summary>
    /// Executa o cálculo de pontos usando Strategy Pattern (UC-09 / RF-006).
    /// </summary>
    /// <param name="strategy">Strategy de cálculo injetada</param>
    /// <param name="fatorConversao">Fator do cartão</param>
    public void CalcularPontos(ICalculoPontosStrategy strategy, decimal fatorConversao)
    {
        if (strategy == null)
        {
            throw new ArgumentNullException(nameof(strategy), "Strategy de cálculo não pode ser nula");
        }

        if (fatorConversao <= 0)
        {
            throw new ValorInvalidoException("Fator de conversão deve ser maior que zero");
        }

        // UC-09: Validação pré-cálculo (evita divisão por zero)
        if (Valor <= 0 || CotacaoDolar <= 0)
        {
            PontosEstimados = 0;
            return;
        }

        // UC-09: Execução da fórmula matemática
        var valorEmDolares = Valor / CotacaoDolar;
        PontosEstimados = strategy.Calcular(valorEmDolares, fatorConversao);
    }

    /// <summary>
    /// Valida todos os campos obrigatórios conforme UC-08 (RF-008).
    /// </summary>
    /// <exception cref="ValorInvalidoException">Lançada quando dados são inválidos</exception>
    public void Validar()
    {
        var erros = new List<string>();

        // UC-08: Verificação de Campos Obrigatórios
        if (string.IsNullOrWhiteSpace(Descricao))
        {
            erros.Add("Descrição da transação é obrigatória");
        }

        if (string.IsNullOrWhiteSpace(Categoria))
        {
            erros.Add("Categoria da transação é obrigatória");
        }

        // UC-08: Verificação de Valores Monetários
        if (Valor <= 0)
        {
            erros.Add("Valor da transação deve ser maior que zero");
        }

        if (CotacaoDolar <= 0)
        {
            erros.Add("Cotação do dólar deve ser maior que zero");
        }

        // UC-08: Verificação de Datas (Não pode ser futura)
        if (Data > DateTime.Now)
        {
            erros.Add("Data da transação não pode ser futura");
        }

        // UC-08: Verificação de Foreign Key
        if (CartaoId <= 0)
        {
            erros.Add("Cartão vinculado é obrigatório");
        }

        // Se houver erros, lança exceção com todas as mensagens
        if (erros.Any())
        {
            throw new ValorInvalidoException(string.Join("; ", erros));
        }
    }
}

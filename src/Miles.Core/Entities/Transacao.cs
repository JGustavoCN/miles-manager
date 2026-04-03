using System.ComponentModel.DataAnnotations;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;

namespace Miles.Core.Entities;

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

    public void CalcularPontos(ICalculoPontosStrategy strategy, decimal fatorConversao)
    {
        if (strategy == null) throw new ArgumentNullException(nameof(strategy));
        if (fatorConversao <= 0) throw new ValorInvalidoException("Fator de conversão deve ser maior que zero");

        if (Valor <= 0 || CotacaoDolar <= 0)
        {
            PontosEstimados = 0;
            return;
        }

        var valorEmDolares = Valor / CotacaoDolar;
        PontosEstimados = strategy.Calcular(valorEmDolares, fatorConversao);
    }

    /// <summary>
    /// Valida os dados da transação conforme UC-08 (RF-008).
    /// Lança ValidationException com lista estruturada de erros.
    /// </summary>
    /// <exception cref="ValidationException">Lançada com todos os erros encontrados.</exception>
    public void Validar()
    {
        var erros = new List<string>();

        // UC-08: Campos obrigatórios
        if (string.IsNullOrWhiteSpace(Descricao))
            erros.Add("Descrição da transação é obrigatória");

        if (string.IsNullOrWhiteSpace(Categoria))
            erros.Add("Categoria da transação é obrigatória");

        // UC-08: Valores monetários > 0
        if (Valor <= 0)
            erros.Add("Valor da transação deve ser maior que zero");

        if (CotacaoDolar <= 0)
            erros.Add("Cotação do dólar deve ser maior que zero");

        // UC-08: Data não pode ser futura
        if (Data > DateTime.Now)
            erros.Add("Data da transação não pode ser futura");

        // UC-08: Foreign Key
        if (CartaoId <= 0)
            erros.Add("Cartão vinculado é obrigatório");

        if (erros.Any())
            throw new ValidationException(erros);
    }

    // --- NOVO MÉTODO PARA UPDATE ---
    public void AtualizarDados(string descricao, decimal valor, DateTime data, string categoria, int cartaoId)
    {
        Descricao = descricao;
        Valor = valor;
        Data = data;
        Categoria = categoria;
        CartaoId = cartaoId;
    }
}

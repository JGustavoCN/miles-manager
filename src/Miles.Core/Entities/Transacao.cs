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
        if (fatorConversao <= 0) throw new ValorInvalidoException("Fator de conversão inválido");

        if (Valor <= 0 || CotacaoDolar <= 0)
        {
            PontosEstimados = 0;
            return;
        }

        var valorEmDolares = Valor / CotacaoDolar;
        PontosEstimados = strategy.Calcular(valorEmDolares, fatorConversao);
    }

    public void Validar()
    {
        var erros = new List<string>();
        if (string.IsNullOrWhiteSpace(Descricao)) erros.Add("Descrição obrigatória");
        if (Valor <= 0) erros.Add("Valor deve ser maior que zero");
        if (CartaoId <= 0) erros.Add("Cartão obrigatório");

        if (erros.Any()) throw new ValorInvalidoException(string.Join("; ", erros));
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

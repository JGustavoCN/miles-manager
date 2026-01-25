using System.ComponentModel.DataAnnotations;
using Miles.Core.Enums;
using Miles.Core.Exceptions;

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

    [Required]
    public CategoriaTransacao Categoria { get; set; }

    [Range(0.01, 100.0)]
    public decimal CotacaoDolar { get; set; }

    public int PontosEstimados { get; set; }

    // Foreign Key
    public int CartaoId { get; set; }
    public virtual Cartao Cartao { get; set; } = null!;

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

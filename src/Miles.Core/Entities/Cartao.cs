using System.ComponentModel.DataAnnotations;
using Miles.Core.Enums;

namespace Miles.Core.Entities;

public class Cartao
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome do cartão é obrigatório")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public BandeiraCartao Bandeira { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Limite deve ser maior que zero")]
    public decimal Limite { get; set; }

    [Range(1, 31, ErrorMessage = "Dia de vencimento deve estar entre 1 e 31")]
    public int DiaVencimento { get; set; }

    [Range(0.01, 10.0, ErrorMessage = "Fator deve estar entre 0.01 e 10")]
    public decimal FatorConversao { get; set; } = 1.0m;

    // Foreign Keys
    public int UsuarioId { get; set; }
    public virtual Usuario Usuario { get; set; } = null!;

    public int ProgramaId { get; set; }
    public virtual ProgramaFidelidade Programa { get; set; } = null!;

    // Relacionamento (1:N)
    public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();

    public bool Validar()
    {
        return !string.IsNullOrWhiteSpace(Nome) &&
               Limite > 0 &&
               DiaVencimento is >= 1 and <= 31 &&
               FatorConversao > 0 &&
               UsuarioId > 0 &&
               ProgramaId > 0;
    }
}

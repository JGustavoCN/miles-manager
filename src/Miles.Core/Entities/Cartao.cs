using System.ComponentModel.DataAnnotations;

namespace Miles.Core.Entities;

/// Representa um cartão de crédito vinculado a um programa de fidelidade (RF-003)

public class Cartao
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome do cartão é obrigatório")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bandeira é obrigatória")]
    [MaxLength(50)]
    public string Bandeira { get; set; } = string.Empty;

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

    /// <summary>
    /// Valida os dados do cartão conforme UC-08 (RF-008)
    /// </summary>
    /// <exception cref="Miles.Core.Exceptions.ValorInvalidoException">Lançada quando dados são inválidos</exception>
    public void Validar()
    {
        var erros = new List<string>();

        // UC-08: Verificação de Campos Obrigatórios
        if (string.IsNullOrWhiteSpace(Nome))
        {
            erros.Add("Nome do cartão é obrigatório");
        }

        if (string.IsNullOrWhiteSpace(Bandeira))
        {
            erros.Add("Bandeira do cartão é obrigatória");
        }

        // UC-08: Verificação de Valores Monetários (Limite deve ser positivo)
        if (Limite <= 0)
        {
            erros.Add("Limite do cartão deve ser maior que zero");
        }

        // UC-08: Verificação de Dia de Vencimento
        if (DiaVencimento < 1 || DiaVencimento > 31)
        {
            erros.Add("Dia de vencimento deve estar entre 1 e 31");
        }

        // UC-08: Verificação de Fator de Conversão
        if (FatorConversao <= 0)
        {
            erros.Add("Fator de conversão deve ser maior que zero");
        }

        // UC-08: Verificação de Foreign Keys
        if (UsuarioId <= 0)
        {
            erros.Add("Usuário vinculado é obrigatório");
        }

        if (ProgramaId <= 0)
        {
            erros.Add("Programa de fidelidade vinculado é obrigatório");
        }

        // Se houver erros, lança exceção com todas as mensagens
        if (erros.Any())
        {
            throw new Miles.Core.Exceptions.ValorInvalidoException(string.Join("; ", erros));
        }
    }
}

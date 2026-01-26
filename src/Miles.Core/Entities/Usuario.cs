using System.ComponentModel.DataAnnotations;

namespace Miles.Core.Entities;

/// Representa o usuário do sistema (RF-002)
/// Ator principal que possui cartões e gerencia programas de fidelidade
public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string SenhaHash { get; set; } = string.Empty;

    // Relacionamentos (1:N)
    public virtual ICollection<Cartao> Cartoes { get; set; } = new List<Cartao>();
    public virtual ICollection<ProgramaFidelidade> Programas { get; set; } = new List<ProgramaFidelidade>();

    /// Valida se os dados essenciais do usuário estão preenchidos (RF-008)
    public bool Validar()
    {
        return !string.IsNullOrWhiteSpace(Nome) &&
               !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(SenhaHash);
    }
}

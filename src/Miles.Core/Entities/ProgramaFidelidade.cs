using System.ComponentModel.DataAnnotations;

namespace Miles.Core.Entities;

/// Representa um programa de milhas/pontos (RF-004)
/// Exemplos: Smiles, Latam Pass, Livelo, Esfera
public class ProgramaFidelidade
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome do programa é obrigatório")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Banco { get; set; } = string.Empty;

    // Foreign Key
    public int UsuarioId { get; set; }
    public virtual Usuario Usuario { get; set; } = null!;

    // Relacionamento (1:N) - Um programa pode receber pontos de vários cartões
    public virtual ICollection<Cartao> Cartoes { get; set; } = new List<Cartao>();

    /// Valida os dados do programa (RF-008)
    public bool Validar()
    {
        return !string.IsNullOrWhiteSpace(Nome) && UsuarioId > 0;
    }
}

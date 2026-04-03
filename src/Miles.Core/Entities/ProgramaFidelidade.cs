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

    /// <summary>
    /// Valida os dados do programa conforme UC-08 (RF-008).
    /// Lança ValidationException com lista estruturada de erros.
    /// </summary>
    /// <exception cref="Miles.Core.Exceptions.ValidationException">Lançada quando dados são inválidos</exception>
    public void Validar()
    {
        var erros = new List<string>();

        // UC-08: Verificação de Campos Obrigatórios
        if (string.IsNullOrWhiteSpace(Nome))
            erros.Add("Nome do programa de fidelidade é obrigatório");

        // UC-08: Verificação de Foreign Key
        if (UsuarioId <= 0)
            erros.Add("Usuário vinculado é obrigatório");

        // Se houver erros, lança ValidationException com lista estruturada
        if (erros.Any())
            throw new Miles.Core.Exceptions.ValidationException(erros);
    }
}

using System.ComponentModel.DataAnnotations;

namespace Miles.Application.DTOs;

public class PerfilInputDTO
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;
}

using System.ComponentModel.DataAnnotations;

namespace Miles.Application.DTOs;

public class TrocaSenhaInputDTO
{
    [Required(ErrorMessage = "Senha atual é obrigatória")]
    public string SenhaAtual { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nova senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public string NovaSenha { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
    [Compare("NovaSenha", ErrorMessage = "A confirmação da senha não confere")]
    public string ConfirmacaoSenha { get; set; } = string.Empty;
}

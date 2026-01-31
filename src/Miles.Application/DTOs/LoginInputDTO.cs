namespace Miles.Application.DTOs;

/// DTO para entrada de dados de login (UC-01)
public class LoginInputDTO
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

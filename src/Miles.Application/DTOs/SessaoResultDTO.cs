namespace Miles.Application.DTOs;

/// DTO para resultado da autenticação (UC-01)
public class SessaoResultDTO
{
    public bool Sucesso { get; set; }
    public string? MensagemErro { get; set; }
    public int? UsuarioId { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
}

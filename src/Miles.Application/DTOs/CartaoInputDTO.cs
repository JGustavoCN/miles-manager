namespace Miles.Application.DTOs;

/// <summary>
/// DTO de entrada para cadastro de cartão (UC-03).
/// </summary>
public class CartaoInputDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Bandeira { get; set; } = string.Empty;
    public decimal Limite { get; set; }
    public int DiaVencimento { get; set; }
    public decimal FatorConversao { get; set; } = 1.0m;
    public int UsuarioId { get; set; }
    public int ProgramaId { get; set; }
}

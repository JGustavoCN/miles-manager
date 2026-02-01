namespace Miles.Application.DTOs;

public class ProgramaInputDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Banco { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
}

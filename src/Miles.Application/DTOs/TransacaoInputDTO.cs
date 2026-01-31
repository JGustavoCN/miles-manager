namespace Miles.Application.DTOs;

/// <summary>
/// DTO de entrada para registro de transação (UC-02).
/// </summary>
public class TransacaoInputDTO
{
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal CotacaoDolar { get; set; }
    public int CartaoId { get; set; }
}

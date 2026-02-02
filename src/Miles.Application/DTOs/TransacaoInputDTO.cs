namespace Miles.Application.DTOs;

public class TransacaoInputDTO
{
    public int? Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal CotacaoDolar { get; set; }
    public int CartaoId { get; set; }

    // Campos novos para exibição na lista (Read-Only context)
    public int PontosEstimados { get; set; }
    public string? NomeCartao { get; set; }
}

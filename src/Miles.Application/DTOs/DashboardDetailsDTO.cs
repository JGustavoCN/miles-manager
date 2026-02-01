namespace Miles.Application.DTOs;

public class DashboardDetailsDTO
{
    public int TotalGeralPontos { get; set; }
    public List<ProgramaSaldoDTO> SaldosPorPrograma { get; set; } = new();
}

public class ProgramaSaldoDTO
{
    public int ProgramaId { get; set; }
    public string NomePrograma { get; set; } = string.Empty;
    public int SaldoTotal { get; set; }
}

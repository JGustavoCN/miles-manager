namespace Miles.Application.DTOs;

public class DashboardDetailsDTO
{
    // KPIs
    public int TotalGeralPontos { get; set; }
    public decimal TotalGastoPeriodo { get; set; }
    public int QuantidadeTransacoes { get; set; }
    public decimal TicketMedio { get; set; } // NOVO: Média de gasto por compra

    // Gráficos
    public List<ProgramaSaldoDTO> SaldosPorPrograma { get; set; } = new();
    public List<CategoriaGastoDTO> GastosPorCategoria { get; set; } = new();
    public List<EvolucaoMensalDTO> EvolucaoPontos { get; set; } = new();

    // NOVO: Top Cartões (para Gráfico de Barras)
    public List<CartaoPerformanceDTO> TopCartoes { get; set; } = new();

    // NOVO: Lista simples para o Mini-Extrato
    public List<TransacaoResumoDTO> UltimasTransacoes { get; set; } = new();
}

// DTOs Auxiliares
public class ProgramaSaldoDTO
{
    public int ProgramaId { get; set; }
    public string NomePrograma { get; set; } = string.Empty;
    public double SaldoTotal { get; set; }
}

public class CategoriaGastoDTO
{
    public string Categoria { get; set; } = string.Empty;
    public double ValorTotal { get; set; }
}

public class EvolucaoMensalDTO
{
    public string MesAno { get; set; } = string.Empty;
    public double PontosAcumulados { get; set; }
}

// NOVAS CLASSES
public class CartaoPerformanceDTO
{
    public string NomeCartao { get; set; } = string.Empty;
    public double PontosGerados { get; set; }
}

public class TransacaoResumoDTO
{
    public DateTime Data { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string NomeCartao { get; set; } = string.Empty;
}

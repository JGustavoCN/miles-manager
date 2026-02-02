using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Interfaces;
using System.Globalization;

namespace Miles.Application.Services;

public class DashboardFacade : IDashboardFacade
{
    private readonly IProgramaRepository _programaRepository;
    private readonly ITransacaoRepository _transacaoRepository;

    public DashboardFacade(IProgramaRepository programaRepository, ITransacaoRepository transacaoRepository)
    {
        _programaRepository = programaRepository;
        _transacaoRepository = transacaoRepository;
    }

    public async Task<DashboardDetailsDTO> ObterDetalhesAsync(int userId)
    {
        // Nota: Certifique-se que seu TransacaoRepository possui o método 'ObterPorUsuarioAsync'
        // Se ele for síncrono (apenas ObterPorUsuario), você precisará ajustar aqui.
        var programas = await _programaRepository.ObterPorUsuarioAsync(userId);
        var transacoes = await _transacaoRepository.ObterPorUsuarioAsync(userId);

        var dashboardDTO = new DashboardDetailsDTO();

        // 1. KPIs Gerais
        // CORREÇÃO: Adicionado () pois Count é um método de extensão do LINQ aqui
        dashboardDTO.QuantidadeTransacoes = transacoes.Count();
        dashboardDTO.TotalGastoPeriodo = transacoes.Sum(t => t.Valor);

        dashboardDTO.TicketMedio = dashboardDTO.QuantidadeTransacoes > 0
        ? dashboardDTO.TotalGastoPeriodo / dashboardDTO.QuantidadeTransacoes
        : 0;

        // 2. Gráfico: Saldos por Programa
        foreach (var programa in programas)
        {
            var saldo = transacoes
                .Where(t => t.Cartao != null && t.Cartao.ProgramaId == programa.Id)
                .Sum(t => t.PontosEstimados);

            if (saldo > 0)
            {
                dashboardDTO.SaldosPorPrograma.Add(new ProgramaSaldoDTO
                {
                    ProgramaId = programa.Id,
                    NomePrograma = programa.Nome,
                    SaldoTotal = (double)saldo
                });
            }
        }
        dashboardDTO.TotalGeralPontos = (int)dashboardDTO.SaldosPorPrograma.Sum(p => p.SaldoTotal);

        // 3. Gráfico: Gastos por Categoria
        var categorias = transacoes
            .GroupBy(t => string.IsNullOrWhiteSpace(t.Categoria) ? "Outros" : t.Categoria)
            .Select(g => new CategoriaGastoDTO
            {
                Categoria = g.Key,
                ValorTotal = (double)g.Sum(t => t.Valor)
            })
            .OrderByDescending(c => c.ValorTotal)
            .Take(5)
            .ToList();

        dashboardDTO.GastosPorCategoria = categorias;

        // 4. Gráfico: Evolução de Pontos (Últimos 6 meses)
        var seisMesesAtras = DateTime.Now.AddMonths(-5);
        var evolucao = transacoes
            .Where(t => t.Data >= new DateTime(seisMesesAtras.Year, seisMesesAtras.Month, 1))
            .GroupBy(t => new { t.Data.Year, t.Data.Month })
            // CORREÇÃO: Ordena pelo ano/mês ANTES de transformar em string para garantir ordem cronológica
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Month)
            .Select(g => new EvolucaoMensalDTO
            {
                MesAno = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key.Month)}/{g.Key.Year.ToString().Substring(2)}",
                PontosAcumulados = (double)g.Sum(t => t.PontosEstimados)
            })
            .ToList();

        dashboardDTO.EvolucaoPontos = evolucao;



        // 5. NOVO: Top Cartões (Quem gera mais pontos?)
        dashboardDTO.TopCartoes = transacoes
            .Where(t => t.Cartao != null)
            .GroupBy(t => t.Cartao.Nome)
            .Select(g => new CartaoPerformanceDTO
            {
                NomeCartao = g.Key,
                PontosGerados = (double)g.Sum(t => t.PontosEstimados)
            })
            .OrderByDescending(c => c.PontosGerados)
            .Take(5)
            .ToList();

        // 6. NOVO: Últimas 5 Transações (Mini Extrato)
        dashboardDTO.UltimasTransacoes = transacoes
            .OrderByDescending(t => t.Data)
            .Take(5)
            .Select(t => new TransacaoResumoDTO
            {
                Data = t.Data,
                Descricao = t.Descricao,
                Valor = t.Valor,
                NomeCartao = t.Cartao?.Nome ?? "N/A"
            })
            .ToList();
        return dashboardDTO;
    }
}

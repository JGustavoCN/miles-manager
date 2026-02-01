using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Interfaces;

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

    public Task<DashboardDetailsDTO> ObterDetalhesAsync(int userId)
    {
        // Passo 2 do Fluxo Principal: Consulta base de dados
        var programas = _programaRepository.ObterPorUsuario(userId);
        var transacoes = _transacaoRepository.ObterPorUsuario(userId);

        var dashboardDTO = new DashboardDetailsDTO();

        // Passo 3 do Fluxo Principal: Processamento de cálculo e agrupamento
        foreach (var programa in programas)
        {
            // Soma pontos das transações vinculadas aos cartões deste programa
            var saldoDoPrograma = transacoes
                .Where(t => t.Cartao != null && t.Cartao.ProgramaId == programa.Id)
                .Sum(t => t.PontosEstimados);

            dashboardDTO.SaldosPorPrograma.Add(new ProgramaSaldoDTO
            {
                ProgramaId = programa.Id,
                NomePrograma = programa.Nome,
                SaldoTotal = saldoDoPrograma
            });
        }

        dashboardDTO.TotalGeralPontos = dashboardDTO.SaldosPorPrograma.Sum(p => p.SaldoTotal);

        return Task.FromResult(dashboardDTO);
    }
}

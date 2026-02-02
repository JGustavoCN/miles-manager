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

    public async Task<DashboardDetailsDTO> ObterDetalhesAsync(int userId)
    {
        // 1. Busca dados de forma Assíncrona (Sem bloquear a thread do servidor)
        // Nota: O DbContext não é thread-safe, por isso fazemos um await de cada vez.
        var programas = await _programaRepository.ObterPorUsuarioAsync(userId);

        // AVISO: Este método ObterPorUsuarioAsync ainda vamos criar no TransacaoRepository a seguir!
        var transacoes = await _transacaoRepository.ObterPorUsuarioAsync(userId);

        var dashboardDTO = new DashboardDetailsDTO();

        // 2. Processamento em memória (LinQ to Objects)
        // Como 'programas' e 'transacoes' já estão materializados na memória, este loop é rápido.
        foreach (var programa in programas)
        {
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

        return dashboardDTO;
    }
}

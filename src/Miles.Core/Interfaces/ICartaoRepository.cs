using Miles.Core.Entities;

namespace Miles.Core.Interfaces;

/// <summary>
/// Contrato de persistência para a entidade Cartao (RF-003)
/// Gerencia cartões de crédito vinculados a programas de fidelidade
/// </summary>
public interface ICartaoRepository
{
    // Leitura: Retorna IEnumerable para indicar que a query já foi materializada
    Task<IEnumerable<Cartao>> ObterTodosAsync(CancellationToken ct = default);

    Task<Cartao?> ObterPorIdAsync(int id, CancellationToken ct = default);

    Task<IEnumerable<Cartao>> ObterPorUsuarioAsync(int userId, CancellationToken ct = default);

    // Escrita: Tudo retorna Task (void assíncrono)
    Task AdicionarAsync(Cartao cartao, CancellationToken ct = default);

    Task AtualizarAsync(Cartao cartao, CancellationToken ct = default);

    Task RemoverAsync(int id, CancellationToken ct = default);

    Task<bool> PossuiTransacoesAsync(int id, CancellationToken ct = default);
}

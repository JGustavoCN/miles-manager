using Miles.Core.Entities;

namespace Miles.Core.Interfaces;

public interface ITransacaoRepository
{
    // Create
    Task AdicionarAsync(Transacao transacao, CancellationToken ct = default);

    // Read
    Task<Transacao?> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Transacao>> ObterPorUsuarioAsync(int userId, CancellationToken ct = default);
    Task<IEnumerable<Transacao>> ObterExtratoAsync(int cartaoId, CancellationToken ct = default);

    // Update
    Task AtualizarAsync(Transacao transacao, CancellationToken ct = default);

    // Delete
    Task RemoverAsync(int id, CancellationToken ct = default);
}

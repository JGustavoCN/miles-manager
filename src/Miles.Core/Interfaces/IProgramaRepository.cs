using Miles.Core.Entities;

namespace Miles.Core.Interfaces;

public interface IProgramaRepository
{
    Task AdicionarAsync(ProgramaFidelidade programa, CancellationToken ct = default);

    Task AtualizarAsync(ProgramaFidelidade programa, CancellationToken ct = default);

    Task RemoverAsync(int id, CancellationToken ct = default);

    Task<ProgramaFidelidade?> ObterPorIdAsync(int id, CancellationToken ct = default);

    Task<IEnumerable<ProgramaFidelidade>> ObterPorUsuarioAsync(int userId, CancellationToken ct = default);

    Task<bool> ExistePeloNomeAsync(string nome, int usuarioId, CancellationToken ct = default);

    Task<bool> PossuiCartoesVinculadosAsync(int id, CancellationToken ct = default);
}

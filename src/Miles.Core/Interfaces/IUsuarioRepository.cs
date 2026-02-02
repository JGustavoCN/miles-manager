using Miles.Core.Entities;

namespace Miles.Core.Interfaces;

/// <summary>
/// Contrato de persistência para a entidade Usuario (RF-002)
/// </summary>
public interface IUsuarioRepository
{
    Task AdicionarAsync(Usuario usuario, CancellationToken ct = default);

    Task AtualizarAsync(Usuario usuario, CancellationToken ct = default);

    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default);

    Task<Usuario?> ObterPorIdAsync(int id, CancellationToken ct = default);
}

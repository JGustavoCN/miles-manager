using Miles.Application.DTOs;

namespace Miles.Application.Interfaces;

public interface ITransacaoService
{
    Task<List<TransacaoInputDTO>> ObterPorUsuarioAsync(int usuarioId);
    Task<TransacaoInputDTO?> ObterPorIdAsync(int id);
    Task AdicionarAsync(TransacaoInputDTO input);
    Task AtualizarAsync(TransacaoInputDTO input);
    Task RemoverAsync(int id);
}

using Miles.Application.DTOs;

namespace Miles.Application.Interfaces;

public interface ICartaoService
{
    Task<List<CartaoInputDTO>> ObterPorUsuarioAsync(int usuarioId);

    Task<CartaoInputDTO?> ObterPorIdAsync(int id);

    // Mudança: void -> Task e renomeado para CadastrarAsync
    Task CadastrarAsync(CartaoInputDTO input);

    // Mudança: void -> Task e renomeado para AtualizarAsync
    Task AtualizarAsync(CartaoInputDTO input);

    Task RemoverAsync(int id);
}

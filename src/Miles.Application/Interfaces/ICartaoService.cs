using Miles.Application.DTOs;

namespace Miles.Application.Interfaces;

public interface ICartaoService
{
    Task<List<CartaoInputDTO>> ObterPorUsuarioAsync(int usuarioId);

    Task<CartaoInputDTO?> ObterPorIdAsync(int id);

    void Cadastrar(CartaoInputDTO input);

    void Atualizar(CartaoInputDTO input);

    Task RemoverAsync(int id);
}

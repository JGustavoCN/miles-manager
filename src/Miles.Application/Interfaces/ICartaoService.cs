using Miles.Application.DTOs;

namespace Miles.Application.Interfaces;

public interface ICartaoService
{
    /// <summary>
    /// Cadastra um novo cartão (UC-03).
    /// </summary>
    /// <exception cref="Miles.Core.Exceptions.ValorInvalidoException">Quando dados são inválidos (UC-08 FE-01)</exception>
    void Cadastrar(CartaoInputDTO input);
}

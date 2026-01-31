using Miles.Application.DTOs;

namespace Miles.Application.Interfaces;

public interface ITransacaoService
{
    /// <summary>
    /// Registra uma nova transação (UC-02).
    /// </summary>
    /// <exception cref="Miles.Core.Exceptions.ValorInvalidoException">Quando dados são inválidos (UC-08 FE-01)</exception>
    void Registrar(TransacaoInputDTO input);
}

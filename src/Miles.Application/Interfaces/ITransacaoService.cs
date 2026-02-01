using Miles.Application.DTOs;

namespace Miles.Application.Interfaces;

public interface ITransacaoService
{
    Task AdicionarAsync(TransacaoInputDTO input);
}

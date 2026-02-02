using Miles.Application.DTOs;

namespace Miles.Application.Interfaces;

public interface IProgramaService
{
    // Apenas métodos Async e retornando DTOs (nada de Entidades aqui)
    Task<List<ProgramaInputDTO>> ObterPorUsuarioAsync(int usuarioId);

    Task<ProgramaInputDTO?> ObterPorIdAsync(int id);

    Task AdicionarAsync(ProgramaInputDTO dto);

    Task AtualizarAsync(ProgramaInputDTO dto);

    Task RemoverAsync(int id);
}

using Miles.Application.DTOs;
using Miles.Core.Entities;

namespace Miles.Application.Interfaces;

public interface IProgramaService
{
    List<ProgramaFidelidade> ListarPorUsuario(int usuarioId);
    ProgramaInputDTO? ObterPorId(int id);
    void Adicionar(ProgramaInputDTO dto);
    void Atualizar(ProgramaInputDTO dto);
    void Remover(int id);
}

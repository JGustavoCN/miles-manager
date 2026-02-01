using Miles.Application.DTOs;
using Miles.Core.Entities;

namespace Miles.Application;

public static class MilesMapper
{
    public static SessaoResultDTO ToResult(Usuario usuario)
    {
        return new SessaoResultDTO
        {
            Sucesso = true,
            UsuarioId = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            MensagemErro = null
        };
    }
    public static SessaoResultDTO ToErrorResult(string mensagem)
    {
        return new SessaoResultDTO
        {
            Sucesso = false,
            MensagemErro = mensagem,
            UsuarioId = null,
            Nome = null,
            Email = null
        };
    }


    public static ProgramaFidelidade ToEntity(ProgramaInputDTO dto)
    {
        return new ProgramaFidelidade
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Banco = dto.Banco,
            UsuarioId = dto.UsuarioId
        };
    }

    // Método auxiliar inverso, útil para carregar o formulário de edição
    public static ProgramaInputDTO ToDTO(ProgramaFidelidade entity)
    {
        return new ProgramaInputDTO
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Banco = entity.Banco,
            UsuarioId = entity.UsuarioId
        };
    }
}

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
}

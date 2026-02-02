using Miles.Application.DTOs;

namespace Miles.Application.Interfaces;

public interface IUsuarioService
{
    Task AtualizarPerfilAsync(int usuarioId, PerfilInputDTO dto);
    Task AlterarSenhaAsync(int usuarioId, TrocaSenhaInputDTO dto);
}

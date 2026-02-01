using Miles.Application.DTOs;

namespace Miles.Application.Interfaces;

public interface IUsuarioService
{
    void AtualizarPerfil(int usuarioId, PerfilInputDTO dto);
    void AlterarSenha(int usuarioId, TrocaSenhaInputDTO dto);
}

using Miles.Core.Entities;
using Miles.Application.DTOs;

namespace Miles.Core.Interfaces;

public interface IAuthService
{
    // Autenticar agora retorna Task (pois vai ao banco buscar a senha hash)
    Task<Usuario?> AutenticarAsync(string email, string senha);

    // O login completo (que retorna o DTO de sessão)
    Task<SessaoResultDTO> RealizarLoginAsync(LoginInputDTO input);

    Task<SessaoResultDTO> RegistrarUsuarioAsync(CadastroInputDTO input);
}

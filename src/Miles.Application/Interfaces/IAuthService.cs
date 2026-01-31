using Miles.Core.Entities;

namespace Miles.Core.Interfaces;

/// Contrato de serviço de autenticação (UC-01)
/// Define operações de login e validação de credenciais
public interface IAuthService
{
    /// Autentica um usuário com email e senha
    /// Retorna o usuário autenticado ou null se as credenciais forem inválidas
    Usuario? Autenticar(string email, string senha);
}

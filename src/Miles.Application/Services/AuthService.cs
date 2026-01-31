using Miles.Application.DTOs;
using Miles.Core.Entities;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;
using BCrypt.Net;

namespace Miles.Application.Services;

/// Serviço de autenticação implementando regras de negócio do UC-01
public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    /// Autentica um usuário validando credenciais
    /// Retorna SessaoResultDTO com resultado da autenticação
    public SessaoResultDTO RealizarLogin(LoginInputDTO input)
    {
        // Validação: Campos obrigatórios
        if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Senha))
        {
            return MilesMapper.ToErrorResult("E-mail e senha são obrigatórios");
        }

        // Buscar usuário por email
        var usuario = _usuarioRepository.ObterPorEmail(input.Email);

        // Validação: Credenciais inválidas
        // Verifica se usuário existe e valida senha com BCrypt
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(input.Senha, usuario.SenhaHash))
        {
            return MilesMapper.ToErrorResult("E-mail ou senha incorretos");
        }

        // Autenticação bem-sucedida
        return MilesMapper.ToResult(usuario);
    }

    /// Implementação da interface IAuthService
    public Usuario? Autenticar(string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
        {
            return null;
        }

        var usuario = _usuarioRepository.ObterPorEmail(email);

        // Valida senha com BCrypt
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash))
        {
            return null;
        }

        return usuario;
    }
}

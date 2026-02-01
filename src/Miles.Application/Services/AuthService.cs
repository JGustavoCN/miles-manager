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

    public async Task<SessaoResultDTO> RegistrarUsuario(CadastroInputDTO input)
    {
        // 1. Verificar se o e-mail já está em uso
        var usuarioExistente = _usuarioRepository.ObterPorEmail(input.Email);
        if (usuarioExistente != null)
        {
            return MilesMapper.ToErrorResult("Este e-mail já está sendo utilizado.");
        }

        // 2. Criar a entidade e realizar o Hash da senha (Segurança)
        var novoUsuario = new Usuario
        {
            Nome = input.Nome,
            Email = input.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(input.Senha)
        };

        // 3. Validar regras de domínio (RF-008)
        try
        {
            novoUsuario.Validar();
        }
        catch (Exception ex)
        {
            return MilesMapper.ToErrorResult(ex.Message);
        }

        // 4. Persistir no banco de dados
        _usuarioRepository.Adicionar(novoUsuario);

        // 5. Retornar sucesso (aproveitando o mapper existente)
        return MilesMapper.ToResult(novoUsuario);
    }


}

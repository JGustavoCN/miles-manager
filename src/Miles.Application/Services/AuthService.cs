using Miles.Application.DTOs;
using Miles.Core.Entities;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;

namespace Miles.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<SessaoResultDTO> RealizarLoginAsync(LoginInputDTO input)
    {
        if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Senha))
            return MilesMapper.ToErrorResult("E-mail e senha são obrigatórios");

        var usuario = await _usuarioRepository.ObterPorEmailAsync(input.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(input.Senha, usuario.SenhaHash))
            return MilesMapper.ToErrorResult("E-mail ou senha incorretos");

        return MilesMapper.ToResult(usuario);
    }

    public async Task<Usuario?> AutenticarAsync(string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            return null;

        var usuario = await _usuarioRepository.ObterPorEmailAsync(email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash))
            return null;

        return usuario;
    }

    public async Task<SessaoResultDTO> RegistrarUsuarioAsync(CadastroInputDTO input)
    {
        // 1. Verificar se e-mail já existe
        var usuarioExistente = await _usuarioRepository.ObterPorEmailAsync(input.Email);
        if (usuarioExistente != null)
            return MilesMapper.ToErrorResult("Este e-mail já está sendo utilizado.");

        // 2. Criar entidade
        var novoUsuario = new Usuario
        {
            Nome = input.Nome,
            Email = input.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(input.Senha)
        };

        // 3. UC-08: Validação Centralizada — catch específico para lista de erros estruturada
        try
        {
            novoUsuario.Validar();
        }
        catch (ValidationException ex)
        {
            // Retorna a lista completa de erros concatenada para o SessaoResultDTO
            return MilesMapper.ToErrorResult(ex.Message);
        }
        catch (ValorInvalidoException ex)
        {
            return MilesMapper.ToErrorResult(ex.Message);
        }

        // 4. Persistir
        await _usuarioRepository.AdicionarAsync(novoUsuario);

        // 5. Retornar sucesso
        return MilesMapper.ToResult(novoUsuario);
    }
}

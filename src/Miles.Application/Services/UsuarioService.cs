using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;

namespace Miles.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task AtualizarPerfilAsync(int usuarioId, PerfilInputDTO dto)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
        if (usuario == null)
            throw new ValorInvalidoException("Usuário não encontrado.");

        // Atualiza apenas o nome
        usuario.Nome = dto.Nome;

        usuario.Validar();

        await _usuarioRepository.AtualizarAsync(usuario);
    }

    public async Task AlterarSenhaAsync(int usuarioId, TrocaSenhaInputDTO dto)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
        if (usuario == null)
            throw new ValorInvalidoException("Usuário não encontrado.");

        // Valida senha atual
        if (!VerificarSenha(dto.SenhaAtual, usuario.SenhaHash))
        {
            throw new ValorInvalidoException("A senha atual informada está incorreta");
        }

        // Gera hash da nova senha
        usuario.SenhaHash = GerarHash(dto.NovaSenha);

        await _usuarioRepository.AtualizarAsync(usuario);
    }

    private bool VerificarSenha(string senhaDigitada, string hashArmazenado)
    {
        return BCrypt.Net.BCrypt.Verify(senhaDigitada, hashArmazenado);
    }

    private string GerarHash(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }
}

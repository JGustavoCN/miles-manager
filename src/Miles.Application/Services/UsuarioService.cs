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

    public void AtualizarPerfil(int usuarioId, PerfilInputDTO dto)
    {
        var usuario = _usuarioRepository.ObterPorId(usuarioId);
        if (usuario == null)
            throw new ValorInvalidoException("Usuário não encontrado.");

        // Atualiza apenas o nome, mantendo o e-mail inalterado (Regra do Checklist)
        usuario.Nome = dto.Nome;

        usuario.Validar(); // Garante integridade (RF-008)
        _usuarioRepository.Atualizar(usuario);
    }

    public void AlterarSenha(int usuarioId, TrocaSenhaInputDTO dto)
    {
        var usuario = _usuarioRepository.ObterPorId(usuarioId);
        if (usuario == null)
            throw new ValorInvalidoException("Usuário não encontrado.");

        // FE-02: Valida se a senha atual está correta
        if (!VerificarSenha(dto.SenhaAtual, usuario.SenhaHash))
        {
            throw new ValorInvalidoException("A senha atual informada está incorreta");
        }

        // Gera o hash da nova senha e atualiza
        usuario.SenhaHash = GerarHash(dto.NovaSenha);

        // Não chamamos usuario.Validar() aqui pois ele validaria nome/email,
        // mas como carregamos do banco, eles devem estar ok.
        _usuarioRepository.Atualizar(usuario);
    }

    // Métodos auxiliares de criptografia (Devem corresponder ao AuthService)
    private bool VerificarSenha(string senhaDigitada, string hashArmazenado)
    {
        // Exemplo usando BCrypt. Se estiver usando outro, ajuste aqui.
        return BCrypt.Net.BCrypt.Verify(senhaDigitada, hashArmazenado);
    }

    private string GerarHash(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }
}

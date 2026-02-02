using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Miles.Application.Services;

/// <summary>
/// Orquestra o caso de uso UC-03 (Manter Cartões).
/// </summary>
public class CartaoService : ICartaoService
{
    private readonly ICartaoRepository _repository;
    private readonly IProgramaRepository _programaRepository;
    private readonly ILogger<CartaoService> _logger;

    public CartaoService(
        ICartaoRepository repository,
        IProgramaRepository programaRepository,
        ILogger<CartaoService> logger)
    {
        _repository = repository;
        _programaRepository = programaRepository;
        _logger = logger;
    }

    public async Task<List<CartaoInputDTO>> ObterPorUsuarioAsync(int usuarioId)
    {
        var cartoes = await _repository.ObterPorUsuarioAsync(usuarioId);
        return cartoes.Select(MilesMapper.ToDTO).ToList();
    }

    public async Task<CartaoInputDTO?> ObterPorIdAsync(int id)
    {
        var cartao = await _repository.ObterPorIdAsync(id);
        return cartao == null ? null : MilesMapper.ToDTO(cartao);
    }

    public async Task CadastrarAsync(CartaoInputDTO input)
    {
        try
        {
            // Alterado para Async: Validação não bloqueante
            await ValidarProgramaExistenteAsync(input.ProgramaId);

            var cartao = MilesMapper.ToEntity(input);

            // UC-08: Validação Centralizada
            cartao.Validar();

            await _repository.AdicionarAsync(cartao);

            _logger.LogInformation("Cartão {Nome} cadastrado com sucesso", cartao.Nome);
        }
        catch (ValorInvalidoException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao cadastrar cartão");
            throw;
        }
    }

    public async Task AtualizarAsync(CartaoInputDTO input)
    {
        try
        {
            // Alterado para Async
            await ValidarProgramaExistenteAsync(input.ProgramaId);

            // 1. Converte DTO para Entidade
            var cartaoAtualizado = MilesMapper.ToEntity(input);

            // 2. Valida regras de domínio
            cartaoAtualizado.Validar();

            // 3. Persistência Async
            await _repository.AtualizarAsync(cartaoAtualizado);

            _logger.LogInformation("Cartão {Id} atualizado com sucesso", cartaoAtualizado.Id);
        }
        catch (ValorInvalidoException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao atualizar cartão");
            throw;
        }
    }

    public async Task RemoverAsync(int id)
    {
        // FE-02: Exclusão Bloqueada
        bool possuiTransacoes = await _repository.PossuiTransacoesAsync(id);

        if (possuiTransacoes)
        {
            var msg = "O cartão não pode ser excluído por possuir compras associadas";
            _logger.LogWarning("Tentativa de excluir cartão {Id} bloqueada: {Msg}", id, msg);
            throw new DomainException(msg);
        }

        await _repository.RemoverAsync(id);

        _logger.LogInformation("Cartão {Id} removido com sucesso", id);
    }

    // Método privado também convertido para Async Task
    private async Task ValidarProgramaExistenteAsync(int programaId)
    {
        // Agora usamos o método assíncrono do repositório de Programas
        var programa = await _programaRepository.ObterPorIdAsync(programaId);

        if (programa == null)
        {
            throw new ValorInvalidoException("Programa de fidelidade não encontrado.");
        }
    }
}

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

    public void Cadastrar(CartaoInputDTO input)
    {
        try
        {
            ValidarProgramaExistente(input.ProgramaId);

            var cartao = MilesMapper.ToEntity(input);

            // UC-08: Validação Centralizada
            cartao.Validar();

            _repository.Adicionar(cartao);
            _logger.LogInformation("Cartão {Nome} cadastrado com sucesso", cartao.Nome);
        }
        catch (ValorInvalidoException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao cadastrar cartão");
            throw;
        }
    }

    public void Atualizar(CartaoInputDTO input)
    {
        try
        {
            ValidarProgramaExistente(input.ProgramaId);

            // 1. Converte DTO para Entidade (cria um objeto novo/detached)
            var cartaoAtualizado = MilesMapper.ToEntity(input);

            // 2. Valida regras de domínio
            cartaoAtualizado.Validar();

            // 3. O Repositório (já corrigido) gerencia o anexo e o update no banco
            _repository.Atualizar(cartaoAtualizado);

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
        // FE-02: Exclusão Bloqueada (Verificação de Integridade)
        // Usa o novo método criado no repositório para verificar transações
        bool possuiTransacoes = await _repository.PossuiTransacoesAsync(id);

        if (possuiTransacoes)
        {
            var msg = "O cartão não pode ser excluído por possuir compras associadas";
            _logger.LogWarning("Tentativa de excluir cartão {Id} bloqueada: {Msg}", id, msg);
            throw new DomainException(msg);
        }

        _repository.Remover(id);
        _logger.LogInformation("Cartão {Id} removido com sucesso", id);
    }

    private void ValidarProgramaExistente(int programaId)
    {
        var programa = _programaRepository.ObterPorId(programaId);
        if (programa == null)
        {
            throw new ValorInvalidoException("Programa de fidelidade não encontrado.");
        }
    }
}

using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Miles.Application.Services;

public class CartaoService : ICartaoService
{
    private readonly ICartaoRepository _cartaoRepository;
    private readonly IProgramaRepository _programaRepository;
    private readonly ILogger<CartaoService> _logger;

    public CartaoService(
        ICartaoRepository cartaoRepository,
        IProgramaRepository programaRepository,
        ILogger<CartaoService> logger)
    {
        _cartaoRepository = cartaoRepository;
        _programaRepository = programaRepository;
        _logger = logger;
    }

    public async Task<List<CartaoInputDTO>> ObterPorUsuarioAsync(int usuarioId)
    {
        var cartoes = await _cartaoRepository.ObterPorUsuarioAsync(usuarioId);
        return cartoes.Select(MilesMapper.ToDTO).ToList();
    }

    public async Task<CartaoInputDTO?> ObterPorIdAsync(int id)
    {
        var cartao = await _cartaoRepository.ObterPorIdAsync(id);
        return cartao != null ? MilesMapper.ToDTO(cartao) : null;
    }

    public async Task CadastrarAsync(CartaoInputDTO input)
    {
        // UC-03 FE-01: Validação de programa existente
        await ValidarProgramaExistenteAsync(input.ProgramaId);

        var cartao = MilesMapper.ToEntity(input);

        // UC-08: Validação Centralizada antes de persistir
        cartao.Validar();

        await _cartaoRepository.AdicionarAsync(cartao);
        _logger.LogInformation("Cartão cadastrado: {Nome}", cartao.Nome);
    }

    public async Task AtualizarAsync(CartaoInputDTO input)
    {
        var cartaoExistente = await _cartaoRepository.ObterPorIdAsync(input.Id);
        if (cartaoExistente == null)
            throw new ValorInvalidoException("Cartão não encontrado.");

        // UC-03 FE-01: Validação de programa existente
        await ValidarProgramaExistenteAsync(input.ProgramaId);

        // Atualiza campos
        cartaoExistente.Nome = input.Nome;
        cartaoExistente.Bandeira = input.Bandeira;
        cartaoExistente.Limite = input.Limite;
        cartaoExistente.DiaVencimento = input.DiaVencimento;
        cartaoExistente.FatorConversao = input.FatorConversao;
        cartaoExistente.ProgramaId = input.ProgramaId;
        cartaoExistente.Usuario = null!;
        cartaoExistente.Programa = null!;

        // UC-08: Validação Centralizada antes de persistir
        cartaoExistente.Validar();

        await _cartaoRepository.AtualizarAsync(cartaoExistente);
        _logger.LogInformation("Cartão atualizado: {Id}", cartaoExistente.Id);
    }

    public async Task RemoverAsync(int id)
    {
        await _cartaoRepository.RemoverAsync(id);
        _logger.LogWarning("Cartão removido: {Id}", id);
    }

    private async Task ValidarProgramaExistenteAsync(int programaId)
    {
        var programa = await _programaRepository.ObterPorIdAsync(programaId);
        if (programa == null)
            throw new ValorInvalidoException("Programa de fidelidade não encontrado.");
    }
}

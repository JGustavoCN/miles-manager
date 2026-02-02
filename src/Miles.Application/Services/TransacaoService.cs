using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Miles.Application.Services;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _repository;
    private readonly ICartaoRepository _cartaoRepository;
    private readonly ITransacaoFactory _factory;
    private readonly ICalculoPontosStrategy _calculoStrategy;
    private readonly ILogger<TransacaoService> _logger;

    public TransacaoService(
        ITransacaoRepository repository,
        ICartaoRepository cartaoRepository,
        ITransacaoFactory factory,
        ICalculoPontosStrategy calculoStrategy,
        ILogger<TransacaoService> logger)
    {
        _repository = repository;
        _cartaoRepository = cartaoRepository;
        _factory = factory;
        _calculoStrategy = calculoStrategy;
        _logger = logger;
    }

    public async Task<List<TransacaoInputDTO>> ObterPorUsuarioAsync(int usuarioId)
    {
        var transacoes = await _repository.ObterPorUsuarioAsync(usuarioId);

        // Mapeamento manual para incluir campos de visualização
        return transacoes.Select(t => new TransacaoInputDTO
        {
            Id = t.Id,
            Data = t.Data,
            Valor = t.Valor,
            Descricao = t.Descricao,
            Categoria = t.Categoria,
            CartaoId = t.CartaoId,
            CotacaoDolar = t.CotacaoDolar,

            // Novos campos corrigidos
            PontosEstimados = t.PontosEstimados,
            NomeCartao = t.Cartao?.Nome
        }).ToList();
    }

    public async Task<TransacaoInputDTO?> ObterPorIdAsync(int id)
    {
        var t = await _repository.ObterPorIdAsync(id);
        if (t == null) return null;

        return new TransacaoInputDTO
        {
            Id = t.Id,
            Data = t.Data,
            Valor = t.Valor,
            Descricao = t.Descricao,
            Categoria = t.Categoria,
            CartaoId = t.CartaoId,
            CotacaoDolar = t.CotacaoDolar,
            PontosEstimados = t.PontosEstimados,
            NomeCartao = t.Cartao?.Nome
        };
    }

    public async Task AdicionarAsync(TransacaoInputDTO input)
    {
        var cartao = await _cartaoRepository.ObterPorIdAsync(input.CartaoId);
        if (cartao == null) throw new ValorInvalidoException("Cartão não encontrado.");

        var dadosTransacao = MilesMapper.ToDadosTransacao(input);
        var transacao = _factory.CriarNova(dadosTransacao);

        transacao.CalcularPontos(_calculoStrategy, cartao.FatorConversao);

        await _repository.AdicionarAsync(transacao);
        _logger.LogInformation("Transação criada: {Descricao}", transacao.Descricao);
    }

    public async Task AtualizarAsync(TransacaoInputDTO input)
    {
        if (!input.Id.HasValue)
            throw new ValorInvalidoException("ID da transação inválido para atualização.");

        // 1. Busca Transação (Vem com o Cartao carregado pelo Include)
        var transacaoExistente = await _repository.ObterPorIdAsync(input.Id.Value);
        if (transacaoExistente == null) throw new ValorInvalidoException("Transação não encontrada.");

        // 2. Busca Cartão (Para pegar o fator de conversão)
        var cartao = await _cartaoRepository.ObterPorIdAsync(input.CartaoId);
        if (cartao == null) throw new ValorInvalidoException("Cartão não encontrado.");

        // 3. Atualiza dados
        transacaoExistente.AtualizarDados(input.Descricao, input.Valor, input.Data, input.Categoria, input.CartaoId);

        // 4. Recalcula pontos
        transacaoExistente.CalcularPontos(_calculoStrategy, cartao.FatorConversao);

        transacaoExistente.Cartao = null!;

        await _repository.AtualizarAsync(transacaoExistente);
        _logger.LogInformation("Transação atualizada: {Id}", input.Id);
    }

    public async Task RemoverAsync(int id)
    {
        await _repository.RemoverAsync(id);
        _logger.LogInformation("Transação removida: {Id}", id);
    }
}

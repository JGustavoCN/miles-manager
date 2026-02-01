using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Entities;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Miles.Application.Services;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _repository;
    private readonly ICartaoRepository _cartaoRepository;
    private readonly ICalculoPontosStrategy _calculoStrategy;
    private readonly ILogger<TransacaoService> _logger;

    public TransacaoService(
        ITransacaoRepository repository,
        ICartaoRepository cartaoRepository,
        ICalculoPontosStrategy calculoStrategy,
        ILogger<TransacaoService> logger)
    {
        _repository = repository;
        _cartaoRepository = cartaoRepository;
        _calculoStrategy = calculoStrategy;
        _logger = logger;
    }

    public async Task AdicionarAsync(TransacaoInputDTO input)
    {
        try
        {
            // CORREÇÃO: Chamada assíncrona ao repositório de cartões
            var cartao = await _cartaoRepository.ObterPorIdAsync(input.CartaoId);

            if (cartao == null)
            {
                throw new ValorInvalidoException("Cartão de crédito não encontrado.");
            }

            // Criação da entidade (Factory ou direto)
            var transacao = new Transacao
            {
                Descricao = input.Descricao,
                Valor = input.Valor,
                Categoria = input.Categoria,
                Data = input.Data,
                CartaoId = input.CartaoId,
                CotacaoDolar = input.CotacaoDolar
            };

            // Regra de Negócio: Calcular Pontos (UC-09)
            transacao.CalcularPontos(_calculoStrategy, cartao.FatorConversao);

            // Validação (UC-08)
            transacao.Validar();

            _repository.Adicionar(transacao);

            _logger.LogInformation("Transação {Descricao} registrada com sucesso", transacao.Descricao);
        }
        catch (ValorInvalidoException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos na transação");
            throw;
        }
    }
}

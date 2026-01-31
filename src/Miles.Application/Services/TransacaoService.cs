using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Entities;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;
using Miles.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Miles.Application.Services;

/// <summary>
/// Orquestra o caso de uso UC-02 (Registrar Transação).
/// </summary>
public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _repository;
    private readonly ITransacaoFactory _factory;
    private readonly ICalculoPontosStrategy _strategy;
    private readonly ICartaoRepository _cartaoRepository;
    private readonly ILogger<TransacaoService> _logger;

    public TransacaoService(
        ITransacaoRepository repository,
        ITransacaoFactory factory,
        ICalculoPontosStrategy strategy,
        ICartaoRepository cartaoRepository,
        ILogger<TransacaoService> logger)
    {
        _repository = repository;
        _factory = factory;
        _strategy = strategy;
        _cartaoRepository = cartaoRepository;
        _logger = logger;
    }

    /// <summary>
    /// Registra uma nova transação (UC-02).
    /// Inclui UC-08 (Validação) e UC-09 (Cálculo).
    /// </summary>
    public void Registrar(TransacaoInputDTO input)
    {
        try
        {
            // Validação de existência do cartão (UC-02 FE-02)
            var cartao = _cartaoRepository.ObterPorId(input.CartaoId);
            if (cartao == null)
            {
                throw new ValorInvalidoException("Cartão não encontrado. Cadastre um cartão antes de registrar transações.");
            }

            // Criação da transação via Factory
            var dados = new DadosTransacao
            {
                Valor = input.Valor,
                Data = input.Data,
                Descricao = input.Descricao,
                Categoria = input.Categoria,
                CotacaoDolar = input.CotacaoDolar,
                CartaoId = input.CartaoId
            };

            var transacao = _factory.CriarNova(dados);

            // UC-08: Validação Centralizada (lança exception se inválido)
            transacao.Validar();

            // UC-09: Cálculo Automático de Pontos
            try
            {
                transacao.CalcularPontos(_strategy, cartao.FatorConversao);
            }
            catch (Exception ex)
            {
                // UC-09 FE-01: Log de erro de cálculo
                _logger.LogWarning(ex, "Erro ao calcular pontos para transação. Atribuindo 0 pontos.");
                transacao.PontosEstimados = 0;
            }

            // Persistência
            _repository.Adicionar(transacao);

            _logger.LogInformation("Transação registrada com sucesso: {Descricao} - {Pontos} pontos",
                transacao.Descricao, transacao.PontosEstimados);
        }
        catch (ValorInvalidoException ex)
        {
            // UC-08 FE-01: Propaga erro de validação para o frontend
            _logger.LogWarning(ex, "Dados inválidos ao registrar transação");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao registrar transação");
            throw new DomainException("Erro ao processar transação. Tente novamente.", ex);
        }
    }
}

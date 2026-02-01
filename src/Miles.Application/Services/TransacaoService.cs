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
    private readonly ITransacaoFactory _factory; // Injeção da Factory
    private readonly ICalculoPontosStrategy _calculoStrategy;
    private readonly ILogger<TransacaoService> _logger;

    public TransacaoService(
        ITransacaoRepository repository,
        ICartaoRepository cartaoRepository,
        ITransacaoFactory factory, // Adicionado no construtor
        ICalculoPontosStrategy calculoStrategy,
        ILogger<TransacaoService> logger)
    {
        _repository = repository;
        _cartaoRepository = cartaoRepository;
        _factory = factory;
        _calculoStrategy = calculoStrategy;
        _logger = logger;
    }

    public async Task AdicionarAsync(TransacaoInputDTO input)
    {
        try
        {
            // Validação de existência do cartão (Regra de Negócio / Integridade)
            var cartao = await _cartaoRepository.ObterPorIdAsync(input.CartaoId);
            if (cartao == null)
            {
                throw new ValorInvalidoException("Cartão de crédito não encontrado.");
            }

            // 1. Converter DTO para ValueObject (Mapper)
            var dadosTransacao = MilesMapper.ToDadosTransacao(input);

            // 2. Criar entidade via Factory (Garante estado consistente inicial)
            var transacao = _factory.CriarNova(dadosTransacao);

            // 3. Regra de Negócio CRÍTICA: Calcular Pontos (UC-09)
            // A factory cria com pontos zerados; a Strategy aplica a regra matemática
            transacao.CalcularPontos(_calculoStrategy, cartao.FatorConversao);

            // 4. Persistir
            _repository.Adicionar(transacao);

            _logger.LogInformation("Transação {Descricao} registrada com sucesso. Pontos estimados: {Pontos}", transacao.Descricao, transacao.PontosEstimados);
        }
        catch (ValorInvalidoException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos na transação: {Message}", ex.Message);
            throw; // Repassa para ser tratado pelo middleware ou controller
        }
    }
}

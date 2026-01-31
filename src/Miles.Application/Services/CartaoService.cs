using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Entities;
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

    /// <summary>
    /// Cadastra um novo cartão (UC-03).
    /// Inclui UC-08 (Validação Centralizada).
    /// </summary>
    public void Cadastrar(CartaoInputDTO input)
    {
        try
        {
            // Validação de existência do programa (UC-03 FE-01)
            var programa = _programaRepository.ObterPorId(input.ProgramaId);
            if (programa == null)
            {
                throw new ValorInvalidoException("Programa de fidelidade não encontrado.");
            }

            var cartao = new Cartao
            {
                Nome = input.Nome,
                Bandeira = input.Bandeira,
                Limite = input.Limite,
                DiaVencimento = input.DiaVencimento,
                FatorConversao = input.FatorConversao,
                UsuarioId = input.UsuarioId,
                ProgramaId = input.ProgramaId
            };

            // UC-08: Validação Centralizada (lança exception se inválido)
            cartao.Validar();

            // Persistência
            _repository.Adicionar(cartao);

            _logger.LogInformation("Cartão cadastrado com sucesso: {Nome}", cartao.Nome);
        }
        catch (ValorInvalidoException ex)
        {
            // UC-08 FE-01: Propaga erro de validação
            _logger.LogWarning(ex, "Dados inválidos ao cadastrar cartão");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao cadastrar cartão");
            throw new DomainException("Erro ao processar cartão. Tente novamente.", ex);
        }
    }
}

using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Miles.Application.Services;

public class ProgramaService : IProgramaService
{
    private readonly IProgramaRepository _repository;
    private readonly ILogger<ProgramaService> _logger;

    public ProgramaService(IProgramaRepository repository, ILogger<ProgramaService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<ProgramaInputDTO>> ObterPorUsuarioAsync(int usuarioId)
    {
        var programas = await _repository.ObterPorUsuarioAsync(usuarioId);
        return programas.Select(MilesMapper.ToDTO).ToList();
    }

    public async Task<ProgramaInputDTO?> ObterPorIdAsync(int id)
    {
        var programa = await _repository.ObterPorIdAsync(id);
        return programa != null ? MilesMapper.ToDTO(programa) : null;
    }

    public async Task AdicionarAsync(ProgramaInputDTO dto)
    {
        // UC-03 FE-01: Validação de duplicidade de nome
        if (await _repository.ExistePeloNomeAsync(dto.Nome, dto.UsuarioId))
            throw new ValorInvalidoException("O programa já se encontra cadastrado.");

        var programa = MilesMapper.ToEntity(dto);

        // UC-08: Validação Centralizada (RF-008)
        programa.Validar();

        await _repository.AdicionarAsync(programa);
        _logger.LogInformation("Programa adicionado: {Nome} para Usuário {UsuarioId}", programa.Nome, programa.UsuarioId);
    }

    public async Task AtualizarAsync(ProgramaInputDTO dto)
    {
        var programaExistente = await _repository.ObterPorIdAsync(dto.Id);
        if (programaExistente == null) return;

        // Validação de nome único
        if (programaExistente.Nome != dto.Nome &&
            await _repository.ExistePeloNomeAsync(dto.Nome, dto.UsuarioId))
            throw new ValorInvalidoException("O programa já se encontra cadastrado.");

        // Atualiza campos
        programaExistente.Nome = dto.Nome;
        programaExistente.Banco = dto.Banco;

        // UC-08: Validação Centralizada
        programaExistente.Validar();

        programaExistente.Usuario = null!;

        await _repository.AtualizarAsync(programaExistente);
        _logger.LogInformation("Programa atualizado: {Id} - {Nome}", programaExistente.Id, programaExistente.Nome);
    }

    public async Task RemoverAsync(int id)
    {
        // UC-04 FE-02: Valida Integridade Referencial
        if (await _repository.PossuiCartoesVinculadosAsync(id))
            throw new ValorInvalidoException("Não é possível excluir o programa devido a vínculos existentes.");

        await _repository.RemoverAsync(id);
        _logger.LogWarning("Programa removido: {Id}", id);
    }
}

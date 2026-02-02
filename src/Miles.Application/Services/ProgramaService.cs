using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;

namespace Miles.Application.Services;

public class ProgramaService : IProgramaService
{
    private readonly IProgramaRepository _repository;

    public ProgramaService(IProgramaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProgramaInputDTO>> ObterPorUsuarioAsync(int usuarioId)
    {
        // O repositório agora retorna IEnumerable, então o Select funciona perfeitamente
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
        // 1. Validar Duplicidade de Nome (FE-01) - Agora é Async!
        if (await _repository.ExistePeloNomeAsync(dto.Nome, dto.UsuarioId))
        {
            throw new ValorInvalidoException("O programa já se encontra cadastrado.");
        }

        var programa = MilesMapper.ToEntity(dto);

        // 2. Validação da Entidade (RF-008)
        programa.Validar();

        await _repository.AdicionarAsync(programa);
    }

    public async Task AtualizarAsync(ProgramaInputDTO dto)
    {
        // 1. Busca do banco (traz o Usuario junto por causa do Include)
        var programaExistente = await _repository.ObterPorIdAsync(dto.Id);
        if (programaExistente == null) return;

        // Validação de nome único...
        if (programaExistente.Nome != dto.Nome &&
            await _repository.ExistePeloNomeAsync(dto.Nome, dto.UsuarioId))
        {
            throw new ValorInvalidoException("O programa já se encontra cadastrado.");
        }

        // Atualiza campos
        programaExistente.Nome = dto.Nome;
        programaExistente.Banco = dto.Banco;

        programaExistente.Validar();

        programaExistente.Usuario = null!;

        await _repository.AtualizarAsync(programaExistente);
    }

    public async Task RemoverAsync(int id)
    {
        // 1. Validar Integridade Referencial (FE-02)
        if (await _repository.PossuiCartoesVinculadosAsync(id))
        {
            throw new ValorInvalidoException("Não é possível excluir o programa devido a vínculos existentes.");
        }

        await _repository.RemoverAsync(id);
    }
}

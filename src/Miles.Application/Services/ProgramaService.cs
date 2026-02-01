using Miles.Application.DTOs;
using Miles.Application.Interfaces;
using Miles.Core.Entities;
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

    public List<ProgramaFidelidade> ListarPorUsuario(int usuarioId)
    {
        return _repository.ObterPorUsuario(usuarioId);
    }

    public Task<List<ProgramaInputDTO>> ObterPorUsuarioAsync(int usuarioId)
    {
        var programas = _repository.ObterPorUsuario(usuarioId);

        var dtos = programas.Select(MilesMapper.ToDTO).ToList();

        return Task.FromResult(dtos);
    }

    public ProgramaInputDTO? ObterPorId(int id)
    {
        var programa = _repository.ObterPorId(id);
        return programa != null ? MilesMapper.ToDTO(programa) : null;
    }

    public void Adicionar(ProgramaInputDTO dto)
    {
        // 1. Validar Duplicidade de Nome (FE-01)
        // Regra: "Assegurando que o nome do programa [...] não seja duplicado"
        if (_repository.ExistePeloNome(dto.Nome, dto.UsuarioId))
        {
            throw new ValorInvalidoException("O programa já se encontra cadastrado.");
        }

        var programa = MilesMapper.ToEntity(dto);

        // 2. Validação da Entidade (RF-008)
        programa.Validar();

        _repository.Adicionar(programa);
    }

    public void Atualizar(ProgramaInputDTO dto)
    {
        var programaExistente = _repository.ObterPorId(dto.Id);
        if (programaExistente == null) return;

        // Se o nome mudou, verificar se o novo nome já existe para outro programa
        if (programaExistente.Nome != dto.Nome &&
            _repository.ExistePeloNome(dto.Nome, dto.UsuarioId))
        {
            throw new ValorInvalidoException("O programa já se encontra cadastrado.");
        }

        // Atualiza campos
        programaExistente.Nome = dto.Nome;
        programaExistente.Banco = dto.Banco;

        programaExistente.Validar();

        _repository.Atualizar(programaExistente);
    }

    public void Remover(int id)
    {
        // 1. Validar Integridade Referencial (FE-02)
        // Regra: "Se existirem cartões vinculados, impedir exclusão e exibir alerta exato"
        if (_repository.PossuiCartoesVinculados(id))
        {
            throw new ValorInvalidoException("Não é possível excluir o programa devido a vínculos existentes.");
        }

        _repository.Remover(id);
    }
}

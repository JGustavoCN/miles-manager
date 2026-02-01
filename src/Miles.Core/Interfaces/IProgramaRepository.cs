using Miles.Core.Entities;

namespace Miles.Core.Interfaces;

/// Contrato de persistência para a entidade ProgramaFidelidade (RF-004)
/// Gerencia programas de milhas como Smiles, Latam Pass, Livelo

public interface IProgramaRepository
{
    void Adicionar(ProgramaFidelidade programa);

    void Atualizar(ProgramaFidelidade programa);

    void Remover(int id);

    ProgramaFidelidade? ObterPorId(int id);

    List<ProgramaFidelidade> ObterPorUsuario(int userId);

    bool ExistePeloNome(string nome, int usuarioId);

    bool PossuiCartoesVinculados(int id);
}

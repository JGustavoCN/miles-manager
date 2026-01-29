using Miles.Core.Entities;

namespace Miles.Core.Interfaces;

/// Contrato de persistência para a entidade Cartao (RF-003)
/// Gerencia cartões de crédito vinculados a programas de fidelidade

public interface ICartaoRepository
{
    Task<List<Cartao>> ObterTodosAsync();

    void Adicionar(Cartao cartao);


    List<Cartao> ObterPorUsuario(int userId);


    Cartao? ObterPorId(int id);


    void Atualizar(Cartao cartao);


    void Remover(int id);
}

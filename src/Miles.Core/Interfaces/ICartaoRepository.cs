using Miles.Core.Entities;

namespace Miles.Core.Interfaces;

/// Contrato de persistência para a entidade Cartao (RF-003)
/// Gerencia cartões de crédito vinculados a programas de fidelidade

public interface ICartaoRepository
{

    Task<List<Cartao>> ObterTodosAsync();

    // Alterado para Async para suportar operações de I/O no banco (EF Core)
    Task<Cartao?> ObterPorIdAsync(int id);

    // Alterado para Async para suportar operações de I/O no banco
    Task<List<Cartao>> ObterPorUsuarioAsync(int userId);

    void Adicionar(Cartao cartao);

    void Atualizar(Cartao cartao);

    void Remover(int id);

    Task<bool> PossuiTransacoesAsync(int id);
}


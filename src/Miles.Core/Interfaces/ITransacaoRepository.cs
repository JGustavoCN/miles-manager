using Miles.Core.Entities;

namespace Miles.Core.Interfaces;

/// Contrato de persistência para a entidade Transacao (RF-005)
/// Gerencia o histórico de compras e cálculo de pontos
public interface ITransacaoRepository
{

    void Adicionar(Transacao transacao);


    List<Transacao> ObterExtrato(int cartaoId);


    List<Transacao> ObterPorUsuario(int userId);
}

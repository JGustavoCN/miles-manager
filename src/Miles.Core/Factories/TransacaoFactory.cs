using Miles.Core.Entities;
using Miles.Core.Interfaces;
using Miles.Core.ValueObjects;

namespace Miles.Core.Factories;

public class TransacaoFactory : ITransacaoFactory
{
    public Transacao CriarNova(DadosTransacao dados)
    {

        if (dados == null)
        {
            throw new ArgumentNullException(nameof(dados), "Dados da transação não podem ser nulos");
        }

        var transacao = new Transacao
        {
            Valor = dados.Valor,
            Data = dados.Data,
            Descricao = dados.Descricao,
            Categoria = dados.Categoria,
            CotacaoDolar = dados.CotacaoDolar,
            CartaoId = dados.CartaoId,
            // PontosEstimados será calculado posteriormente via Strategy
            PontosEstimados = 0
        };

        // Validação de Regras de Negócio (RF-008)
        // Este método lança DomainException se houver inconsistências
        transacao.Validar();

        return transacao;
    }
}

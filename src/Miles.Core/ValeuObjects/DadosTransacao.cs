namespace Miles.Core.ValueObjects;

/// Value Object para transferência de dados de transação (Parameter Object Pattern)
/// Encapsula os campos obrigatórios fornecidos pelo usuário no formulário de registro
/// Este objeto serve como contrato de entrada para a ITransacaoFactory.
/// Ele NÃO contém dados calculados (PontosEstimados) nem relacionamentos (Cartao).
/// Esses valores são preenchidos pela Factory e pela Strategy posteriormente.
public class DadosTransacao
{

    public decimal Valor { get; set; }


    public DateTime Data { get; set; }


    public string Descricao { get; set; } = string.Empty;


    public string Categoria { get; set; } = string.Empty;

        public decimal CotacaoDolar { get; set; }

       public int CartaoId { get; set; }
}

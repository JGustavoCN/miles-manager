using System.ComponentModel.DataAnnotations;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;

namespace Miles.Core.Entities;

/// <summary>
/// Entidade central do domínio que representa uma transação financeira.
/// Adere ao princípio de Rica Anêmica, com validações embutidas (UC-08).
/// </summary>
public class Transacao : BaseEntity
{
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
    public string Descricao { get; set; } = string.Empty;

    // UC-08: Regra restaurada - Categoria
    public string Categoria { get; set; } = string.Empty;

    public int CartaoId { get; set; }
    public Cartao? Cartao { get; set; }

    public int? FaturaId { get; set; }

    public decimal CotacaoDolar { get; set; }
    public int PontosEstimados { get; set; }

    // Campos opcionais
    public int? Parcelas { get; set; }
    public int? ParcelaAtual { get; set; }
    public string StatusFatura { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? ComprovanteUrl { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime? DataAtualizacao { get; set; }

    /// <summary>
    /// Calcula os pontos gerados por essa compra específica (RF-009).
    /// </summary>
    public void CalcularPontos(ICalculoPontosStrategy strategy, decimal fatorConversaoCartao)
    {
        PontosEstimados = strategy.Calcular(Valor, CotacaoDolar, fatorConversaoCartao);
    }

    /// <summary>
    /// Valida os dados da transação conforme UC-08 (RF-008).
    /// Lança ValidationException com lista estruturada de erros.
    /// </summary>
    /// <exception cref="Miles.Core.Exceptions.ValidationException">Lançada com todos os erros encontrados.</exception>
    public void Validar()
    {
        var erros = new List<string>();

        // UC-08: Data futura não é permitida (REGRESSÃO CORRIGIDA)
        if (Data.Date > DateTime.Now.Date)
            erros.Add("Data da transação não pode ser futura");

        if (Valor <= 0)
            erros.Add("Valor da transação deve ser maior que zero");

        // UC-08: Cotação não pode ser <= 0 (REGRESSÃO CORRIGIDA)
        if (CotacaoDolar <= 0)
            erros.Add("Cotação do dólar deve ser maior que zero");

        if (string.IsNullOrWhiteSpace(Descricao))
            erros.Add("Descrição da transação é obrigatória");

        // UC-08: Categoria da transação (REGRESSÃO CORRIGIDA)
        if (string.IsNullOrWhiteSpace(Categoria))
            erros.Add("Categoria da transação é obrigatória");

        if (CartaoId <= 0)
            erros.Add("Cartão vinculado é obrigatório");

        if (erros.Any())
            throw new Miles.Core.Exceptions.ValidationException(erros);
    }

    // --- NOVO MÉTODO PARA UPDATE ---
    /// <summary>
    /// Atualiza os dados da transação e reconstrói o objeto atualizando a Data de Update.
    /// Chamado pela Application Layer.
    /// </summary>
    public void AtualizarDados(DateTime data, decimal valor, string descricao, string categoria, int cartaoId)
    {
        Data = data;
        Valor = valor;
        Descricao = descricao;
        Categoria = categoria;
        CartaoId = cartaoId;
        DataAtualizacao = DateTime.Now;

        // Ao alterar os dados fundamentais, precisamos revalidar
        Validar();
    }
}

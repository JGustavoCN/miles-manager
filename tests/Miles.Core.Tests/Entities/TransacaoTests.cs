using Miles.Core.Entities;
using Miles.Core.Exceptions;
using Miles.Core.Interfaces;
using Miles.Core.Strategies;
using Xunit;

namespace Miles.Core.Tests.Entities;

public class TransacaoTests
{
    private readonly ICalculoPontosStrategy _strategy = new CalculoPadraoStrategy();

    [Fact]
    public void Validar_DeveGerarErro_QuandoDataEFutura()
    {
        // Arrange - UC-08: Data futura não é permitida
        var transacao = new Transacao
        {
            Data = DateTime.Now.AddDays(1),
            Valor = 100.00m,
            Descricao = "Teste",
            Categoria = "Teste",
            CotacaoDolar = 5.00m,
            CartaoId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => transacao.Validar());
        Assert.Contains("Data da transação não pode ser futura", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarErro_QuandoValorEhNegativo()
    {
        // Arrange - UC-08: Valores negativos não são permitidos
        var transacao = new Transacao
        {
            Data = DateTime.Now,
            Valor = -100.00m,
            Descricao = "Teste",
            Categoria = "Teste",
            CotacaoDolar = 5.00m,
            CartaoId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => transacao.Validar());
        Assert.Contains("Valor da transação deve ser maior que zero", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarErro_QuandoValorEhZero()
    {
        // Arrange - UC-08: Valor zero não é permitido
        var transacao = new Transacao
        {
            Data = DateTime.Now,
            Valor = 0m,
            Descricao = "Teste",
            Categoria = "Teste",
            CotacaoDolar = 5.00m,
            CartaoId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => transacao.Validar());
        Assert.Contains("Valor da transação deve ser maior que zero", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarErro_QuandoCotacaoEhZero()
    {
        // Arrange - UC-08: Cotação zero não é permitida
        var transacao = new Transacao
        {
            Data = DateTime.Now,
            Valor = 100.00m,
            Descricao = "Teste",
            Categoria = "Teste",
            CotacaoDolar = 0m,
            CartaoId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => transacao.Validar());
        Assert.Contains("Cotação do dólar deve ser maior que zero", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarErro_QuandoDescricaoEstaVazia()
    {
        // Arrange - UC-08: Descrição é obrigatória
        var transacao = new Transacao
        {
            Data = DateTime.Now,
            Valor = 100.00m,
            Descricao = "",
            Categoria = "Teste",
            CotacaoDolar = 5.00m,
            CartaoId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => transacao.Validar());
        Assert.Contains("Descrição da transação é obrigatória", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarErro_QuandoCategoriaEstaVazia()
    {
        // Arrange - UC-08: Categoria é obrigatória
        var transacao = new Transacao
        {
            Data = DateTime.Now,
            Valor = 100.00m,
            Descricao = "Teste",
            Categoria = "",
            CotacaoDolar = 5.00m,
            CartaoId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => transacao.Validar());
        Assert.Contains("Categoria da transação é obrigatória", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarMultiplosErros_QuandoVariosProblemas()
    {
        // Arrange - UC-08: Múltiplos erros devem ser retornados
        var transacao = new Transacao
        {
            Data = DateTime.Now.AddDays(1),
            Valor = 0m,
            Descricao = "",
            Categoria = "",
            CotacaoDolar = 0m,
            CartaoId = 0
        };

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => transacao.Validar());

        // UC-08: Verifica lista estruturada de erros via Errors property
        Assert.Contains("Descrição da transação é obrigatória", exception.Errors);
        Assert.Contains("Categoria da transação é obrigatória", exception.Errors);
        Assert.Contains("Valor da transação deve ser maior que zero", exception.Errors);
        Assert.Contains("Cotação do dólar deve ser maior que zero", exception.Errors);
        Assert.Contains("Data da transação não pode ser futura", exception.Errors);
        Assert.Contains("Cartão vinculado é obrigatório", exception.Errors);

        // Verifica que a lista tem exatamente 6 erros
        Assert.Equal(6, exception.Errors.Count);
    }

    [Fact]
    public void Validar_DevePassar_QuandoDadosValidos()
    {
        // Arrange - UC-08: Dados válidos não geram exceção
        var transacao = new Transacao
        {
            Data = DateTime.Now,
            Valor = 100.00m,
            Descricao = "Compra válida",
            Categoria = "Alimentação",
            CotacaoDolar = 5.00m,
            CartaoId = 1
        };

        // Act & Assert
        var exception = Record.Exception(() => transacao.Validar());
        Assert.Null(exception);
    }

    [Fact]
    public void CalcularPontos_DeveRetornarZero_QuandoCotacaoEhZero()
    {
        // Arrange - UC-09 FE-01: Divisão por zero deve retornar 0 pontos
        var transacao = new Transacao
        {
            Valor = 100.00m,
            CotacaoDolar = 0m
        };

        // Act
        transacao.CalcularPontos(_strategy, 1.0m);

        // Assert
        Assert.Equal(0, transacao.PontosEstimados);
    }

    [Fact]
    public void CalcularPontos_DeveRetornarZero_QuandoValorEhZero()
    {
        // Arrange - UC-09 FE-01: Valor zero deve retornar 0 pontos
        var transacao = new Transacao
        {
            Valor = 0m,
            CotacaoDolar = 5.00m
        };

        // Act
        transacao.CalcularPontos(_strategy, 1.0m);

        // Assert
        Assert.Equal(0, transacao.PontosEstimados);
    }

    [Fact]
    public void CalcularPontos_DeveAplicarFormula_Corretamente()
    {
        // Arrange - UC-09: Fórmula (100 / 5.00) * 1.5 = 30 pontos
        var transacao = new Transacao
        {
            Valor = 100.00m,
            CotacaoDolar = 5.00m
        };

        // Act
        transacao.CalcularPontos(_strategy, 1.5m);

        // Assert
        Assert.Equal(30, transacao.PontosEstimados);
    }

    [Fact]
    public void CalcularPontos_DeveArredondarParaBaixo()
    {
        // Arrange - UC-09: Arredondamento (100 / 5.01) * 1.5 = 29.94 -> 29 pontos
        var transacao = new Transacao
        {
            Valor = 100.00m,
            CotacaoDolar = 5.01m
        };

        // Act
        transacao.CalcularPontos(_strategy, 1.5m);

        // Assert (100 / 5.01 * 1.5 = 29.94...)
        Assert.Equal(29, transacao.PontosEstimados);
    }

    [Fact]
    public void CalcularPontos_DeveLancarErro_QuandoStrategyEhNula()
    {
        // Arrange
        var transacao = new Transacao
        {
            Valor = 100.00m,
            CotacaoDolar = 5.00m
        };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => transacao.CalcularPontos(null!, 1.0m));
    }

    [Fact]
    public void CalcularPontos_DeveLancarErro_QuandoFatorEhZero()
    {
        // Arrange
        var transacao = new Transacao
        {
            Valor = 100.00m,
            CotacaoDolar = 5.00m
        };

        // Act & Assert
        var exception = Assert.Throws<ValorInvalidoException>(() => transacao.CalcularPontos(_strategy, 0m));
        Assert.Contains("Fator de conversão deve ser maior que zero", exception.Message);
    }
}

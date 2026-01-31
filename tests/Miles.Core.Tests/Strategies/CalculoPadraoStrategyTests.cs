using Miles.Core.Strategies;
using Xunit;

namespace Miles.Core.Tests.Strategies;

public class CalculoPadraoStrategyTests
{
    private readonly CalculoPadraoStrategy _strategy = new();

    [Fact]
    public void Calcular_DeveRetornarZero_QuandoValorDolaresEhZero()
    {
        // Arrange - UC-09 FE-01
        var valorDolares = 0m;
        var fator = 1.5m;

        // Act
        var resultado = _strategy.Calcular(valorDolares, fator);

        // Assert
        Assert.Equal(0, resultado);
    }

    [Fact]
    public void Calcular_DeveRetornarZero_QuandoValorDolaresEhNegativo()
    {
        // Arrange - UC-09 FE-01
        var valorDolares = -20m;
        var fator = 1.5m;

        // Act
        var resultado = _strategy.Calcular(valorDolares, fator);

        // Assert
        Assert.Equal(0, resultado);
    }

    [Fact]
    public void Calcular_DeveRetornarZero_QuandoFatorEhZero()
    {
        // Arrange - UC-09 FE-01
        var valorDolares = 20m;
        var fator = 0m;

        // Act
        var resultado = _strategy.Calcular(valorDolares, fator);

        // Assert
        Assert.Equal(0, resultado);
    }

    [Fact]
    public void Calcular_DeveRetornarZero_QuandoFatorEhNegativo()
    {
        // Arrange - UC-09 FE-01
        var valorDolares = 20m;
        var fator = -1.5m;

        // Act
        var resultado = _strategy.Calcular(valorDolares, fator);

        // Assert
        Assert.Equal(0, resultado);
    }

    [Fact]
    public void Calcular_DeveArredondarParaBaixo()
    {
        // Arrange - UC-09: 99.87 * 1.5 = 149.805 deve virar 149
        var valorDolares = 99.87m;
        var fator = 1.5m;

        // Act
        var resultado = _strategy.Calcular(valorDolares, fator);

        // Assert
        Assert.Equal(149, resultado);
    }

    [Fact]
    public void Calcular_DeveAplicarFormulaCorretamente()
    {
        // Arrange - UC-09: 20 * 1.5 = 30
        var valorDolares = 20m;
        var fator = 1.5m;

        // Act
        var resultado = _strategy.Calcular(valorDolares, fator);

        // Assert
        Assert.Equal(30, resultado);
    }

    [Fact]
    public void Calcular_DeveAplicarFormulaComFatorUm()
    {
        // Arrange - UC-09: 50 * 1.0 = 50
        var valorDolares = 50m;
        var fator = 1.0m;

        // Act
        var resultado = _strategy.Calcular(valorDolares, fator);

        // Assert
        Assert.Equal(50, resultado);
    }

    [Theory]
    [InlineData(100, 1.0, 100)]
    [InlineData(100, 1.5, 150)]
    [InlineData(100, 2.0, 200)]
    [InlineData(50.5, 1.2, 60)]  // 50.5 * 1.2 = 60.6 -> 60
    [InlineData(33.33, 3.0, 99)] // 33.33 * 3.0 = 99.99 -> 99
    public void Calcular_DeveAplicarFormulaCorretamente_ComVariosValores(decimal valorDolares, decimal fator, int pontosEsperados)
    {
        // Act
        var resultado = _strategy.Calcular(valorDolares, fator);

        // Assert
        Assert.Equal(pontosEsperados, resultado);
    }
}

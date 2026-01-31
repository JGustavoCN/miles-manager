using Miles.Core.Entities;
using Miles.Core.Exceptions;
using Xunit;

namespace Miles.Core.Tests.Entities;

public class CartaoTests
{
    [Fact]
    public void Validar_DeveGerarErro_QuandoNomeEstaVazio()
    {
        // Arrange - UC-08
        var cartao = new Cartao
        {
            Nome = "",
            Bandeira = "Visa",
            Limite = 1000m,
            DiaVencimento = 10,
            FatorConversao = 1.0m,
            UsuarioId = 1,
            ProgramaId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ValorInvalidoException>(() => cartao.Validar());
        Assert.Contains("Nome do cartão é obrigatório", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarErro_QuandoLimiteEhZero()
    {
        // Arrange - UC-08
        var cartao = new Cartao
        {
            Nome = "Cartão Teste",
            Bandeira = "Mastercard",
            Limite = 0m,
            DiaVencimento = 15,
            FatorConversao = 1.0m,
            UsuarioId = 1,
            ProgramaId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ValorInvalidoException>(() => cartao.Validar());
        Assert.Contains("Limite do cartão deve ser maior que zero", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarErro_QuandoDiaVencimentoInvalido()
    {
        // Arrange - UC-08
        var cartao = new Cartao
        {
            Nome = "Cartão Teste",
            Bandeira = "Elo",
            Limite = 5000m,
            DiaVencimento = 35, // Inválido
            FatorConversao = 1.0m,
            UsuarioId = 1,
            ProgramaId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ValorInvalidoException>(() => cartao.Validar());
        Assert.Contains("Dia de vencimento deve estar entre 1 e 31", exception.Message);
    }

    [Fact]
    public void Validar_DevePassar_QuandoDadosValidos()
    {
        // Arrange - UC-08
        var cartao = new Cartao
        {
            Nome = "Cartão Platinum",
            Bandeira = "Visa",
            Limite = 10000m,
            DiaVencimento = 20,
            FatorConversao = 1.5m,
            UsuarioId = 1,
            ProgramaId = 1
        };

        // Act & Assert
        var exception = Record.Exception(() => cartao.Validar());
        Assert.Null(exception);
    }
}

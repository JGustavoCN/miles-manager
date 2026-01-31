using Miles.Core.Entities;
using Miles.Core.Exceptions;
using Xunit;

namespace Miles.Core.Tests.Entities;

public class UsuarioTests
{
    [Fact]
    public void Validar_DeveGerarErro_QuandoNomeEstaVazio()
    {
        // Arrange - UC-08
        var usuario = new Usuario
        {
            Nome = "",
            Email = "teste@exemplo.com",
            SenhaHash = "hash123"
        };

        // Act & Assert
        var exception = Assert.Throws<ValorInvalidoException>(() => usuario.Validar());
        Assert.Contains("Nome do usuário é obrigatório", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarErro_QuandoEmailEstaVazio()
    {
        // Arrange - UC-08
        var usuario = new Usuario
        {
            Nome = "João Silva",
            Email = "",
            SenhaHash = "hash123"
        };

        // Act & Assert
        var exception = Assert.Throws<ValorInvalidoException>(() => usuario.Validar());
        Assert.Contains("E-mail do usuário é obrigatório", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarErro_QuandoEmailInvalido()
    {
        // Arrange - UC-08
        var usuario = new Usuario
        {
            Nome = "João Silva",
            Email = "emailinvalido", // Sem @
            SenhaHash = "hash123"
        };

        // Act & Assert
        var exception = Assert.Throws<ValorInvalidoException>(() => usuario.Validar());
        Assert.Contains("E-mail em formato inválido", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarErro_QuandoSenhaEstaVazia()
    {
        // Arrange - UC-08
        var usuario = new Usuario
        {
            Nome = "João Silva",
            Email = "joao@exemplo.com",
            SenhaHash = ""
        };

        // Act & Assert
        var exception = Assert.Throws<ValorInvalidoException>(() => usuario.Validar());
        Assert.Contains("Senha do usuário é obrigatória", exception.Message);
    }

    [Fact]
    public void Validar_DevePassar_QuandoDadosValidos()
    {
        // Arrange - UC-08
        var usuario = new Usuario
        {
            Nome = "Maria Santos",
            Email = "maria@exemplo.com",
            SenhaHash = "hashsenha456"
        };

        // Act & Assert
        var exception = Record.Exception(() => usuario.Validar());
        Assert.Null(exception);
    }
}

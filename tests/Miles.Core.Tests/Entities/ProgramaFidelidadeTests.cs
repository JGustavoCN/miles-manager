using Miles.Core.Entities;
using Miles.Core.Exceptions;
using Xunit;

namespace Miles.Core.Tests.Entities;

public class ProgramaFidelidadeTests
{
    [Fact]
    public void Validar_DeveGerarErro_QuandoNomeEstaVazio()
    {
        // Arrange - UC-08
        var programa = new ProgramaFidelidade
        {
            Nome = "",
            UsuarioId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ValorInvalidoException>(() => programa.Validar());
        Assert.Contains("Nome do programa de fidelidade é obrigatório", exception.Message);
    }

    [Fact]
    public void Validar_DeveGerarErro_QuandoUsuarioIdInvalido()
    {
        // Arrange - UC-08
        var programa = new ProgramaFidelidade
        {
            Nome = "Smiles",
            UsuarioId = 0
        };

        // Act & Assert
        var exception = Assert.Throws<ValorInvalidoException>(() => programa.Validar());
        Assert.Contains("Usuário vinculado é obrigatório", exception.Message);
    }

    [Fact]
    public void Validar_DevePassar_QuandoDadosValidos()
    {
        // Arrange - UC-08
        var programa = new ProgramaFidelidade
        {
            Nome = "Latam Pass",
            Banco = "Itaú",
            UsuarioId = 1
        };

        // Act & Assert
        var exception = Record.Exception(() => programa.Validar());
        Assert.Null(exception);
    }
}

using Miles.Core.Interfaces;

namespace Miles.Core.Strategies;

public class CalculoPadraoStrategy : ICalculoPontosStrategy
{
    public int Calcular(decimal valorDolares, decimal fator)
    {
        if (valorDolares <= 0 || fator <= 0)
        {
            return 0; // Retorna 0 pontos em caso de valores inválidos
        }

        var pontosCalculados = valorDolares * fator;

        return (int)Math.Floor(pontosCalculados);
    }
}

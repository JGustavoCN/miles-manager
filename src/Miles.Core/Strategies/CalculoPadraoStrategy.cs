using Miles.Core.Interfaces;

namespace Miles.Core.Strategies;

/// <summary>
/// Implementação padrão do cálculo de pontos (UC-09 / RF-006).
/// Fórmula: (Valor em Dólares) × Fator de Conversão
/// </summary>
public class CalculoPadraoStrategy : ICalculoPontosStrategy
{
    /// <summary>
    /// Calcula pontos estimados com tratamento robusto de divisão por zero (UC-09).
    /// </summary>
    /// <param name="valorDolares">Valor da transação em dólares</param>
    /// <param name="fator">Fator de conversão do cartão</param>
    /// <returns>Pontos estimados (inteiro)</returns>
    public int Calcular(decimal valorDolares, decimal fator)
    {
        // UC-09 FE-01: Tratamento de Divisão por Zero ou Valores Inválidos
        if (valorDolares <= 0 || fator <= 0)
        {
            // Retorna 0 pontos como valor seguro (não quebra a aplicação)
            return 0;
        }

        // UC-09: Execução da Fórmula Matemática
        // Fórmula: (Valor USD) × Fator
        var pontosCalculados = valorDolares * fator;

        // UC-09: Arredondamento para baixo (Floor)
        // Exemplo: 149.8 pontos → 149 pontos
        return (int)Math.Floor(pontosCalculados);
    }
}

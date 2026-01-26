namespace Miles.Core.Interfaces;

/// Strategy para cálculo de pontos/milhas (RF-006)
/// Permite trocar algoritmos de conversão sem modificar a entidade Transacao
/// Exemplo: Cálculo Nacional vs Internacional, Promoções Especiais
/// <remarks>
/// Implementa o padrão Strategy (GoF) para garantir o Open/Closed Principle:
/// - Aberto para extensão: Novas estratégias podem ser criadas
/// - Fechado para modificação: A entidade Transacao não precisa mudar
/// </remarks>
public interface ICalculoPontosStrategy
{

    int Calcular(decimal valorReais, decimal cotacaoDolar, decimal fatorConversao);
}

namespace Miles.Core.Exceptions;

/// Exceção lançada quando valores financeiros ou numéricos são inválidos.
/// Exemplos: Valor negativo, cotação inválida, limite zerado.
public class ValorInvalidoException : DomainException
{
    public ValorInvalidoException(string message) : base(message) { }
}

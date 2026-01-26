namespace Miles.Core.Exceptions;

/// Exceção base para erros de domínio.
/// Permite que a camada de apresentação capture erros de negócio específicos.
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }

    public DomainException(string message, Exception innerException)
        : base(message, innerException) { }
}

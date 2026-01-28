namespace Miles.Core.Exceptions;

/// Exceção lançada quando valores de domínio são inválidos (RF-008).
public class ValorInvalidoException : DomainException
{
    public ValorInvalidoException(string mensagem) : base(mensagem)
    {
    }

    public ValorInvalidoException(string mensagem, Exception inner) : base(mensagem, inner)
    {
    }
}

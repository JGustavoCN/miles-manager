namespace Miles.Core.Exceptions;

/// <summary>
/// Exceção lançada quando múltiplas regras de validação falham (UC-08).
/// Permite retornar uma lista de erros ao usuário de forma estruturada.
/// </summary>
public class ValidationException : DomainException
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("Erros de validação detectados")
    {
        Errors = errors.ToList().AsReadOnly();
    }

    public ValidationException(string singleError)
        : base(singleError)
    {
        Errors = new List<string> { singleError }.AsReadOnly();
    }

    /// <summary>
    /// Retorna todas as mensagens de erro concatenadas.
    /// </summary>
    public override string Message => string.Join("; ", Errors);
}

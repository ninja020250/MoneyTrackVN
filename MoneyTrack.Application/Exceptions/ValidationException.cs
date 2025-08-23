using FluentValidation.Results;

namespace MoneyTrack.Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(ValidationResult validationResult)
    {
        ValidationErrors = new List<string>();
        foreach (var validationError in validationResult.Errors) ValidationErrors.Add(validationError.ErrorMessage);
    }

    public ValidationException()
    {
    }

    public List<string> ValidationErrors { get; set; }
}
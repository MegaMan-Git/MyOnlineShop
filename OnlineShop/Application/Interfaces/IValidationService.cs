using FluentValidation.Results;

namespace Application.Interfaces
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateAsync<T>(T modelDto);
    }
}

using FluentValidation.Results;

namespace Application.Interfaces
{
    public interface IValidationService
    {
        /// <summary>
        /// مربوطه را اعتبارسنجی میکند Validator
        /// </summary>
        /// <typeparam name="T">نوع مدل مورد اعتبارسنجی</typeparam>
        /// <param name="modelDto">مدل مورد اعتبارسنجی</param>
        /// <returns>نتیجه اعتبارسنجی</returns>
        Task<ValidationResult> ValidateAsync<T>(T modelDto);
    }
}

using Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    /// <summary>
    /// مناسب مدل را از سرویس های ثبت شده دریافت و اجرا میکند Validator
    /// </summary>
    public class ValidationService : IValidationService
    {

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ها دریافت میکند Validator سرویس دهنده وابستگی را برای یافتن
        /// </summary>
        /// <param name="serviceProvider">ارائه دهنده سرویس های ثبت شده در برنامه</param>
        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// مربوطه را اعتبارسنجی میکند Validator
        /// </summary>
        /// <typeparam name="T">نوع مدل مورد اعتبارسنجی</typeparam>
        /// <param name="modelDto">مدل مورد اعتبارسنجی</param>
        /// <returns>نتیجه اعتبارسنجی</returns>
        public async Task<ValidationResult> ValidateAsync<T>(T modelDto)
        {
            var validation = _serviceProvider.GetRequiredService<IValidator<T>>();

            return await validation.ValidateAsync(modelDto);
        }
    }
}

using Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class ValidationService : IValidationService
    {

        private readonly IServiceProvider _serviceProvider;
        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ValidationResult> ValidateAsync<T>(T modelDto)
        {
            var validation = _serviceProvider.GetRequiredService<IValidator<T>>();

            return await validation.ValidateAsync(modelDto);
        }
    }
}

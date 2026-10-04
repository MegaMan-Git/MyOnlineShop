using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateAsync<T>(T modelDto);
    }
}

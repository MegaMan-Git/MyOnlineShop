using Application.Dtos.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Auth
{
    internal class ForgotPasswordDtoValidator: AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidator()
        {
            RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".فیلد ایمیل وارد نشده")
                .EmailAddress().WithMessage(".فرمت ایمیل وارده صحیح نمیباشد");
        }
    }
}

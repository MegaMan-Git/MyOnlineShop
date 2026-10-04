using Application.Dtos.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Auth
{
    public class ResetPasswordDtoValidator: AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".فیلد ایمیل وارد نشده")
                .EmailAddress().WithMessage(".فرمت ایمیل وارده صحیح نمیباشد");

            RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".لطفا رمز عبور خود را وارد کنید")
                .MinimumLength(6).WithMessage(".رمز کوتاه هست")
                .Equal(p => p.ConfirmedPassword).WithMessage(".تکرار رمز عبور صحیح نمیباشد");
        }
    }
}

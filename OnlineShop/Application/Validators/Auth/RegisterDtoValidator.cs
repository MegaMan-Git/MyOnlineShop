using Application.Dtos.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Auth
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(p => p.UserName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".نام کاربری وارد نشده")
                .MinimumLength(4).WithMessage(".نام کاربری کوتاه هست");
            
            RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".فیلد ایمیل وارد نشده")
                .EmailAddress().WithMessage(".فرمت ایمیل وارده صحیح نمیباشد");
 
            RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".لطفا رمز را وارد کنید")
                .MinimumLength(6).WithMessage(".رمز کوتاه هست");

            RuleFor(p => p.PhoneNumber).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".شماره موبایل وارد نشده")
                .Matches(@"^09\d{9}$").WithMessage(".فرمت شماره موبایل باید 09123456789 باشد");
        }
    }
}

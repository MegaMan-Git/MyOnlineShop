using Application.Dtos.Auth;
using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Auth
{
    public class RoleChangeDtoValidator: AbstractValidator<RoleChangeDto>
    {
        public RoleChangeDtoValidator()
        {
            RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".ایمیل نمیتواند خالی باشد")
                .EmailAddress().WithMessage(".فرمت ایمیل صحیح نمیباشد");

            RuleFor(p => p.Role).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".فیلد رول وارد نشده")
                .Must(value => Enum.TryParse<Roles>(value, true, out _))
                .WithMessage("Admin, Seller, Customer :مقدار ارسالی باید شامل یکی از این سه مورد باشد");
        }
    }
}

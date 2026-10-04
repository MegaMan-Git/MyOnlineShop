using Application.Dtos.Payment;
using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Payment
{
    public class AdminChangeStatusDtoValidator: AbstractValidator<AdminChangeStatusDto>
    {
        public AdminChangeStatusDtoValidator()
        {
            RuleFor(p => p.OrderId).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".آیدی سفارش وارد نشده است")
                .GreaterThan(0).WithMessage(".آیدی ارسالی نادرست است");

            RuleFor(p => p.Status).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(".فیلد وضعیت پرداختی ارسال نشده است")
            .Must(value => Enum.TryParse<PaymentStatus>(value,true,out _))
            .WithMessage("Waiting, Completed, Expired :مقدار ارسالی باید شامل یکی از این سه مورد باشد");
        }
    }
}

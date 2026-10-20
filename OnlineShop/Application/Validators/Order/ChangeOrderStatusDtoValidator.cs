using Application.Dtos.Order;
using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Order
{
    public class ChangeOrderStatusDtoValidator : AbstractValidator<ChangeOrderStatusDto>
    {
        public ChangeOrderStatusDtoValidator()
        {
            RuleFor(p => p.OrderId)
                .NotEmpty().WithMessage(".فیلد شناسه سفارش وارد نشده است");

            RuleFor(p => p.Status).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".فیلد وضعیت سفارش وارد نشده است")
                .Must(value => Enum.GetNames<OrderStatus>()
                    .Where(x => x is "Paid" or "Processing" or "Shipped" or "Delivered" or "Cancelled")
                    .Contains(value?.Trim(), StringComparer.OrdinalIgnoreCase))
                .WithMessage("Paid ,Processing ,Shipped ,Delivered ,Cancelled :مقدار ارسالی باید شامل یکی از این موارد باشد");
        }
    }
}

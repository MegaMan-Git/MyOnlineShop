using Application.Dtos.Cart.Cartitem;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Cart
{
    public class UpdateCartItemDtoValidator: AbstractValidator<UpdateCartItemDto>
    {
        public UpdateCartItemDtoValidator()
        {
            RuleFor(p => p.CartItemId).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".آیدی کارت آیتم وارد نشده است")
                .GreaterThan(0).WithMessage(".مقدار آیدی نادرست وارد شده است");

            RuleFor(p => p.Quantity).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".تعداد محصول درخواستی مورد نظر ارسال نشده است")
                .GreaterThan(0).WithMessage(".تعداد محصول درخواستی نمیتواند کمتر از صفر باشد");
        }
    }
}

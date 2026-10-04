using Application.Dtos.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Product
{
    public class UpdateProductDtoValidator: AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(p => p.Id).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".آیدی محصول وارد نشده است")
                .GreaterThan(0).WithMessage(".مقدار آیدی نادرست وارد شده است");

            RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage(".نام محصول وارد نشده است");

            RuleFor(p => p.Price).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".قیمت محصول وارد نشده است")
                .GreaterThan(0).WithMessage(".قیمت محصول نمیتواند صفر باشد");

            RuleFor(p => p.Stock).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".موجودی محصول وارد نشده است")
                .GreaterThan(0).WithMessage(".موجودی محصول نمیتواند صفر باشد");

            RuleFor(p => p.CategoryId).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".دسته بندی محصول انتخاب نشده است")
                .GreaterThan(0).WithMessage(".آیدی دسته بندی وجود ندارد");
        }
    }
}

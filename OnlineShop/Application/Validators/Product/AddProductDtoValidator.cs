using Application.Dtos.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Product
{
    public class AddProductDtoValidator: AbstractValidator<AddProductDto>
    {
        public AddProductDtoValidator()
        {
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

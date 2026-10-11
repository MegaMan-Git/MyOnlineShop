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
                .GreaterThan(0).WithMessage(".آیدی محصول به درستی وارد نشده است");

            RuleFor(p => p.ProductName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".نام محصول وارد نشده است")
                .MaximumLength(100).WithMessage(".نام محصول نمیتواند بیشتر از 100 کاراکتر باشد");

            RuleFor(p => p.Price).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".قیمت محصول وارد نشده است")
                .GreaterThan(0).WithMessage(".قیمت محصول نمیتواند صفر باشد");

            RuleFor(p => p.Stock).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".موجودی محصول وارد نشده است")
                .GreaterThan(0).WithMessage(".موجودی محصول نمیتواند صفر باشد");

            RuleFor(p => p.CategoryId).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".دسته بندی محصول انتخاب نشده است");
        }
    }
}

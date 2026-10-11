using Application.Dtos.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Category
{
    public class AddCategoryDtoValidator: AbstractValidator<AddCategoryDto>
    {
        public AddCategoryDtoValidator()
        {
            RuleFor(p => p.CategoryName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".فیلد نام دسته بندی وارد نشده است")
                .MaximumLength(200).WithMessage(".نام دسته بندی بیشتر از 200 کاراکتر نمیتواند باشد");
        }
    }
}

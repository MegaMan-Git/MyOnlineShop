using Application.Dtos.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Category
{
    public class UpdateCategoryDtoValidator: AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage(".آیدی دسته بندی وارد نشده است");

            RuleFor(p => p.NewCategoryName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".فیلد نام دسته بندی وارد نشده است")
                .MaximumLength(200).WithMessage(".نام دسته بندی بیشتر از 200 کاراکتر نمیتواند باشد");
        }
    }
}

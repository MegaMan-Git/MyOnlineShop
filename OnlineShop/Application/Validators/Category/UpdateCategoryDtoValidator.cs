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
            RuleFor(p => p.Id).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(".آیدی دسته بندی وارد نشده است")
                .GreaterThan(0).WithMessage(".مقدار آیدی نادرست وارد شده است");

            RuleFor(p => p.NewCategoryName).NotEmpty();  
        }
    }
}

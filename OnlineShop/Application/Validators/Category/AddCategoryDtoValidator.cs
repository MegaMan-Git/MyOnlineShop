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
            RuleFor(p => p.CategoryName).NotEmpty();
        }
    }
}

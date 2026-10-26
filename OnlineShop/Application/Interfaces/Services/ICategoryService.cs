using Application.Dtos.Category;
using Application.Filters;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<ServiceResult<IEnumerable<CategoryDto>>> GetAllCategoriesAsync
            (QueryStringParameters parameters);
        Task<ServiceResult<CategoryDto>> GetCategoryByIdAsync(int categoryId);

        Task<ServiceResult<CategoryDto>> AddCategoryAsync(AddCategoryDto categoryDto);

        Task<ServiceResult<CategoryDto>> UpdateCategoryAsync(UpdateCategoryDto categoryDto);

        Task<ServiceResult<string>> SoftDeleteCategoryAsync(int categoryId);
    }
}

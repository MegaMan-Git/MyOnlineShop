using Application.Dtos.Category;
using Application.Entities;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<ServiceResult<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
        Task<ServiceResult<CategoryDto>> GetCategoryByIdAsync(int categoryId);

        Task<ServiceResult<CategoryDto>> AddCategoryAsync(AddCategoryDto categoryDto);

        Task<ServiceResult<CategoryDto>> UpdateCategoryAsync(UpdateCategoryDto categoryDto);

        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}

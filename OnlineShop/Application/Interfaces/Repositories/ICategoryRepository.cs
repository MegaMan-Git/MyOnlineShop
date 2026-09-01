using Application.Dtos.Category;
using Application.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task<bool> IsDuplicateCategoryNameAsync(string categoryName);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}

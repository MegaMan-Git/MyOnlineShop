using Application.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);

        Task AddCategoryAsync(Category category);
        
        Task UpdateCategoryAsync(Category category);
        
        Task DeleteCategoryAsync(Category category);
        
        Task<bool> IsDuplicateCategoryNameAsync(string categoryName);
    }
}

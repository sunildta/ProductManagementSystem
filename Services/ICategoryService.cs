using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface ICategoryService
    {
        Task<(bool Success, string Message)> AddCategoryAsync(Category category);
        Task<(bool Success, string Message)> UpdateCategoryAsync(Category category);
        Task<(bool Success, string Message)> DeleteCategoryAsync(int id);
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
    }
}

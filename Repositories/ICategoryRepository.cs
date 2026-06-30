using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repositories
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<bool> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
        Task<Category?> GetByNameAsync(string name);
    }
}

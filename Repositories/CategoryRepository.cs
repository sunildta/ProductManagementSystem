using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repositories
{
    /// <summary>
    /// EF Core implementation of ICategoryRepository.
    /// Scoped lifetime — shares DbContext within a DI scope.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) => _db = db;

        public async Task AddAsync(Category category)
        {
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
        }

        public async Task<Category?> GetByIdAsync(int id) =>
            await _db.Categories.FindAsync(id);

        public async Task<IEnumerable<Category>> GetAllAsync() =>
            await _db.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();

        public async Task<bool> UpdateAsync(Category category)
        {
            var existing = await _db.Categories.FindAsync(category.Id);
            if (existing is null) return false;

            existing.Name = category.Name;
            existing.Description = category.Description;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category is null) return false;

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Category?> GetByNameAsync(string name) =>
            await _db.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }
}

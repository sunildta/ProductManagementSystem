using Microsoft.Extensions.Logging;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;
using ProductManagementSystem.Validation;

namespace ProductManagementSystem.Services
{
    /// <summary>
    /// Business logic layer for Category management.
    /// Validates before writes, delegates to ICategoryRepository.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IValidationService _validator;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository repo,
            IValidationService validator,
            ILogger<CategoryService> logger)
        {
            _repo = repo;
            _validator = validator;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> AddCategoryAsync(Category category)
        {
            if (!_validator.Validate(category, out var errors))
            {
                string msg = string.Join(" | ", errors);
                _logger.LogWarning("AddCategory validation failed: {Errors}", msg);
                return (false, msg);
            }

            await _repo.AddAsync(category);
            _logger.LogInformation("Category added: Id={Id} Name={Name}", category.Id, category.Name);
            return (true, $"Category \"{category.Name}\" added successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateCategoryAsync(Category category)
        {
            if (!_validator.Validate(category, out var errors))
                return (false, string.Join(" | ", errors));

            bool updated = await _repo.UpdateAsync(category);
            if (!updated) return (false, $"Category ID {category.Id} not found.");

            _logger.LogInformation("Category updated: Id={Id}", category.Id);
            return (true, $"Category \"{category.Name}\" updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteCategoryAsync(int id)
        {
            bool deleted = await _repo.DeleteAsync(id);
            if (!deleted) return (false, $"Category ID {id} not found.");

            _logger.LogInformation("Category deleted: Id={Id}", id);
            return (true, $"Category ID {id} deleted.");
        }

        public Task<Category?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<IEnumerable<Category>> GetAllAsync() => _repo.GetAllAsync();
    }
}

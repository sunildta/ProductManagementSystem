using Microsoft.Extensions.Logging;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;
using ProductManagementSystem.Validation;

namespace ProductManagementSystem.Services
{
    /// <summary>
    /// Business logic layer for Supplier management.
    /// Validates before writes, delegates to ISupplierRepository.
    /// </summary>
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repo;
        private readonly IValidationService _validator;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(
            ISupplierRepository repo,
            IValidationService validator,
            ILogger<SupplierService> logger)
        {
            _repo = repo;
            _validator = validator;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> AddSupplierAsync(Supplier supplier)
        {
            if (!_validator.Validate(supplier, out var errors))
            {
                string msg = string.Join(" | ", errors);
                _logger.LogWarning("AddSupplier validation failed: {Errors}", msg);
                return (false, msg);
            }

            await _repo.AddAsync(supplier);
            _logger.LogInformation("Supplier added: Id={Id} Name={Name}", supplier.Id, supplier.Name);
            return (true, $"Supplier \"{supplier.Name}\" added successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateSupplierAsync(Supplier supplier)
        {
            if (!_validator.Validate(supplier, out var errors))
                return (false, string.Join(" | ", errors));

            bool updated = await _repo.UpdateAsync(supplier);
            if (!updated) return (false, $"Supplier ID {supplier.Id} not found.");

            _logger.LogInformation("Supplier updated: Id={Id}", supplier.Id);
            return (true, $"Supplier \"{supplier.Name}\" updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteSupplierAsync(int id)
        {
            bool deleted = await _repo.DeleteAsync(id);
            if (!deleted) return (false, $"Supplier ID {id} not found.");

            _logger.LogInformation("Supplier deleted: Id={Id}", id);
            return (true, $"Supplier ID {id} deleted.");
        }

        public Task<Supplier?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<IEnumerable<Supplier>> GetAllAsync() => _repo.GetAllAsync();
    }
}

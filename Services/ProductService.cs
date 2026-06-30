using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text;
using ProductManagementSystem.Validation;
using ProductManagementSystem.Configuration;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;


namespace ProductManagementSystem.Services
{
    /// <summary>
    /// Business logic layer sitting between MenuService and ProductRepository.
    /// Demonstrates:
    ///   - Constructor injection (IProductRepository, IValidationService, IOptions, ILogger)
    ///   - Configuration-driven behaviour via IOptions&lt;AppSettings&gt;
    ///   - Validation before every write operation
    /// </summary>
    class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IValidationService _validator;
        private readonly IOptionsMonitor<AppSettings> _options;
        private readonly ILogger<ProductService> _logger;

        //// All dependencies injected by the DI container — no "new" anywhere
        public ProductService(
            IProductRepository repo,
            IValidationService validator,
            IOptionsMonitor<AppSettings> options,
            ILogger<ProductService> logger)
        {
            _repo = repo;
            _validator = validator;
            _options = options;
            _logger = logger;
        }

        // ── Validated CRUD ────────────────────────────────────────────────────────

        public async Task<(bool Success, string Message)> AddProductAsync(Product product)
        {
            if (!_validator.Validate(product, out var errors))
            {
                string msg = string.Join(" | ", errors);
                _logger.LogWarning("AddProduct validation failed: {Errors}", msg);
                return (false, msg);
            }

            await _repo.AddAsync(product);
            _logger.LogInformation("Product added: Id={Id} Name={Name}", product.Id, product.Name);
            return (true, $"Product \"{product.Name}\" added successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateProductAsync(Product product)
        {
            if (!_validator.Validate(product, out var errors))
                return (false, string.Join(" | ", errors));

            bool updated = await _repo.UpdateAsync(product);
            if (!updated) return (false, $"Product ID {product.Id} not found.");

            _logger.LogInformation("Product updated: Id={Id}", product.Id);
            return (true, $"Product \"{product.Name}\" updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteProductAsync(int id)
        {
            bool deleted = await _repo.DeleteAsync(id);
            if (!deleted) return (false, $"Product ID {id} not found.");

            _logger.LogInformation("Product deleted: Id={Id}", id);
            return (true, $"Product ID {id} deleted.");
        }

        // ── Reads ─────────────────────────────────────────────────────────────────

        public Task<Product?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<Product?> GetByIdWithDetailsAsync(int id) => _repo.GetByIdWithDetailsAsync(id);
        public Task<IEnumerable<Product>> GetAllAsync() => _repo.GetAllAsync();
        public Task<IEnumerable<Product>> GetByCategoryAsync(string category) => _repo.GetByCategoryAsync(category);
        public Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal min, decimal max) => _repo.GetByPriceRangeAsync(min, max);
        public Task<IEnumerable<Product>> GetSortedByPriceAsync(bool ascending = true) => _repo.GetSortedByPriceAsync(ascending);
        public Task<IEnumerable<Product>> GetTopNMostExpensiveAsync(int n = 5) => _repo.GetTopNMostExpensiveAsync(n);
        public Task<decimal> GetTotalInventoryValueAsync() => _repo.GetTotalInventoryValueAsync();

        public Task<IEnumerable<Product>> GetLowStockProductsAsync() =>
            _repo.GetLowStockAsync(_options.CurrentValue.MinimumStockLevel);

        // ── Business Logic (sync — pure math, no DB) ──────────────────────────────

        public decimal CalculatePriceWithTax(decimal price) =>
            Math.Round(price * (1 + _options.CurrentValue.TaxRate), 2);

        public decimal CalculateDiscountedPrice(decimal price) =>
            Math.Round(price * (1 - _options.CurrentValue.DiscountPercentage / 100m), 2);



    }
}

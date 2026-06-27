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
    internal class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IValidationService _validator;
        private readonly AppSettings _settings;
        private readonly ILogger<ProductService> _logger;

        //// All dependencies injected by the DI container — no "new" anywhere
        public ProductService(
            IProductRepository repo,
            IValidationService validator,
            IOptions<AppSettings> options,
            ILogger<ProductService> logger)
        {
            _repo = repo;
            _validator = validator;
            _settings = options.Value;
            _logger = logger;
        }

        //Validate CRUD
        public (bool Success, string Message) AddProduct(Product product)
        {
            if (!_validator.Validate(product, out var errors))
            {
                string msg = string.Join(" | ", errors);
                _logger.LogWarning("AddProduct validation failed: {Errors}", msg);
                return (false, msg);
            }

            _repo.Add(product);
            _logger.LogInformation("Product added: Id={Id}, Name={Name}", product.Id, product.Name);
            return (true, $"Product \"{product.Name}\" added sucessfully.");

        }

        public (bool Success, string Message) UpdateProduct(Product product)
        {
            if (!_validator.Validate(product, out var errors))
                return (false, string.Join(" | ", errors));
            bool updated = _repo.Update(product);
            if (!updated)
                return (false, $"Product Id {product.Id} not found.");

            _logger.LogInformation("Product updated: Id={Id}", product.Id);
            return (true, $"Product \"{ product.Name}\" updated successfully.");
        }
        public (bool Success, String Message) DeleteProduct(int id)
        {
            bool deleted = _repo.Delete(id);
            if (!deleted)
                return (false, $"Product Id {id} not found.");

            _logger.LogInformation("Product deleted:m Id={Id}", id);
            return (true, $"Product ID {id} deleted");
        }
        // -Read query (pass through repo)
        public Product? GetById(int id) => _repo.GetById(id);
        public IEnumerable<Product> GetAll() => _repo.GetAll();
        public IEnumerable<Product> GetByCategory(string category) => _repo.GetByCategory(category);
        public IEnumerable<Product> GetByPriceRange(decimal min, decimal max) => _repo.GetByPriceRange(min, max);
        public IEnumerable<Product> GetSortedByPrice(bool ascending = true) => _repo.GetSortedByPrice(ascending);
        public IEnumerable<Product> GetTopNMostExpensive(int n = 5) => _repo.GetTopNMostExpensive(n);
        public decimal GetTotalInventoryValue() => _repo.GetTotalInventoryValue();

        // Business Logic Using IOption<AppSetting>
        /// <summary>Price + tax from appsettings (TaxRate = 0.18 → 18%).</summary>
        public decimal CalculatePriceWithTax(decimal price) => Math.Round(price * (1 + _settings.TaxRate),2);

        /// <summary>Price after discount from appsettings (DiscountPercentage = 10 → 10% off).</summary>
        public decimal CalculateDiscountedPrice(decimal price) => Math.Round(price * (1 - _settings.DiscountRate/100m),2);

        /// <summary>Products at or below MinimumStockLevel from appsettings.</summary>
        public IEnumerable<Product> GetLowStockProducts() =>
            _repo.GetAll()
            .Where(p => p.Stock <= _settings.MinimumStockLevel)
            .OrderBy(p => p.Stock);
    


    }
}

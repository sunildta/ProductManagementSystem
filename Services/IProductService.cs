using ProductManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Services
{
    public interface IProductService
    {
        // ── Validated async CRUD ──────────────────────────────────────────────────
        Task<(bool Success, string Message)> AddProductAsync(Product product);
        Task<(bool Success, string Message)> UpdateProductAsync(Product product);
        Task<(bool Success, string Message)> DeleteProductAsync(int id);

        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();

        // ── Queries ───────────────────────────────────────────────────────────────
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
        Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal min, decimal max);
        Task<IEnumerable<Product>> GetSortedByPriceAsync(bool ascending = true);
        Task<IEnumerable<Product>> GetTopNMostExpensiveAsync(int n = 5);

        // ── Business logic ────────────────────────────────────────────────────────
        Task<decimal> GetTotalInventoryValueAsync();
        Task<IEnumerable<Product>> GetLowStockProductsAsync();
        decimal CalculatePriceWithTax(decimal price);
        decimal CalculateDiscountedPrice(decimal price);

    }
}

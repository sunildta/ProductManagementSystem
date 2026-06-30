using ProductManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Repositories
{
    public interface IProductRepository
    {
        // ── Async CRUD ────────────────────────────────────────────────────────────
        Task AddAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);

        // ── Async LINQ Queries ────────────────────────────────────────────────────
        Task<IEnumerable<Product>> GetByCategoryAsync(string categoryName);
        Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal min, decimal max);
        Task<IEnumerable<Product>> GetSortedByPriceAsync(bool ascending = true);
        Task<decimal> GetTotalInventoryValueAsync();
        Task<IEnumerable<Product>> GetTopNMostExpensiveAsync(int n = 5);
        Task<IEnumerable<Product>> GetLowStockAsync(int threshold);

        // ── With related data ─────────────────────────────────────────────────────
        Task<Product?> GetByIdWithDetailsAsync(int id); // includes Category, Supplier, Reviews
    }
}

 
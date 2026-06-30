using ProductManagementSystem.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ProductManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace ProductManagementSystem.Repositories
{
    /// <summary>
    /// EF Core implementation of IProductRepository.
    /// DbContext is injected as Scoped — one instance per DI scope (request).
    /// AsNoTracking() used on read-only queries for better performance.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) => _db = db;

        // CRUD
        public async Task AddAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
  
        }

        public async Task<Product?> GetByIdAsync (int id) =>await _db.Products
            .Include(p => p.Category)   // eager load
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Product>> GetAllAsync() => await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .AsNoTracking() //coz read only
            .OrderBy(p => p.Name)
            .ToListAsync();

        public async Task<bool> UpdateAsync(Product product)
        {
            var existing = await _db.Products.FindAsync(product.Id);
            if (existing is null) return false;
            //update field a user can change
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.CategoryId = product.CategoryId;
            existing.SupplierId = product.SupplierId;

            await _db.SaveChangesAsync();
            return true;

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null) return false;

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }


        // ── LINQ Queries ──────────────────────────────────────────────────────────

        /// <summary>Find all products in a given category (case-insensitive).</summary>
        public async Task<IEnumerable<Product>> GetByCategoryAsync(string categoryName) => await _db.Products
             .Include(p => p.Category)
             .Include(p => p.Supplier)
             .AsNoTracking()
             .Where(p => p.Category.Name.ToLower() == categoryName.ToLower())
             .OrderBy(p => p.Name)
             .ToListAsync();

        /// <summary>Find products whose price falls within [min, max].</summary>
        public async Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal min, decimal max) => await _db.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => p.Price >= min && p.Price <= max)
            .OrderBy(p => p.Price)
            .ToListAsync();

        /// <summary>Return all products sorted by price.</summary>
        public async Task<IEnumerable<Product>> GetSortedByPriceAsync(bool ascending = true)
        {
            var query = _db.Products
                       .Include(p => p.Category)
                       .AsNoTracking();

            return await (ascending
                ? query.OrderBy(p => p.Price)
                : query.OrderByDescending(p => p.Price))
                .ToListAsync();
        }

        /// <summary>Sum of (Price × Stock) across every product.</summary>
        public async Task<decimal> GetTotalInventoryValueAsync() => await _db
            .Products.SumAsync(p => p.Price * p.Stock);

        /// <summary>Return the N most expensive products.</summary>
        public async Task<IEnumerable<Product>> GetTopNMostExpensiveAsync(int n = 5) =>
          await _db.Products
                   .Include(p => p.Category)
                   .AsNoTracking()
                   .OrderByDescending(p => p.Price)
                   .Take(n)
                   .ToListAsync();

        //low product threshold
        public async Task<IEnumerable<Product>> GetLowStockAsync(int threshold) =>
            await _db.Products
             .Include(p => p.Category)
             .AsNoTracking()
             .Where(p => p.Stock <= threshold)
             .OrderBy(p => p.Stock)
             .ToListAsync();

        // ── With all related data (for detail view) ───────────────────────────────
        public async Task<Product?> GetByIdWithDetailsAsync(int id) =>
            await _db.Products
                     .Include(p => p.Category)
                     .Include(p => p.Supplier)
                     .Include(p => p.Reviews)   // explicit load of reviews
                     .FirstOrDefaultAsync(p => p.Id == id);
    }
}

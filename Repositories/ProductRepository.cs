using ProductManagementSystem.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();
        private int _nextId = 1;

        // CRUD
        public void Add(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
        }

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);
        public IEnumerable<Product> GetAll() => _products.AsReadOnly();
        public bool Update(Product product)
        {
            int index = _products.FindIndex(p => p.Id == product.Id);
            if (index == -1) return false;
            _products[index] = product;
            return true;
        }
        public bool Delete(int id)
        {
            Product? product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null) return false;
            _products.Remove(product);
            return true;
        }
        // ── LINQ Queries ──────────────────────────────────────────────────────────

        /// <summary>Find all products in a given category (case-insensitive).</summary>
        public IEnumerable<Product> GetByCategory(string category) =>
            _products
                .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.Name);

        /// <summary>Find products whose price falls within [min, max].</summary>
        public IEnumerable<Product> GetByPriceRange(decimal min, decimal max) =>
            _products
                .Where(p => p.Price >= min && p.Price <= max)
                .OrderBy(p => p.Price);

        /// <summary>Return all products sorted by price.</summary>
        public IEnumerable<Product> GetSortedByPrice(bool ascending = true) =>
            ascending
                ? _products.OrderBy(p => p.Price)
                : _products.OrderByDescending(p => p.Price);

        /// <summary>Sum of (Price × Stock) across every product.</summary>
        public decimal GetTotalInventoryValue() =>
            _products.Sum(p => p.Price * p.Stock);

        /// <summary>Return the N most expensive products.</summary>
        public IEnumerable<Product> GetTopNMostExpensive(int n = 5) =>
            _products
                .OrderByDescending(p => p.Price)
                .Take(n);
    }
}

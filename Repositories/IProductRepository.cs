using ProductManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Repositories
{
    public interface IProductRepository
    {
        // CRUD
        void Add(Product product);
        Product? GetById(int id);
        IEnumerable<Product> GetAll();
        bool Update(Product product);
        bool Delete(int id);
       
        // LINQ Queries
        IEnumerable<Product> GetByCategory(string category);
        IEnumerable<Product> GetByPriceRange(decimal min, decimal max);
        IEnumerable<Product> GetSortedByPrice(bool ascending = true);
        decimal GetTotalInventoryValue();
        IEnumerable<Product> GetTopNMostExpensive(int n = 5);
    }
}

 
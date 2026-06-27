using ProductManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Services
{
    public interface IProductService
    {
        //validate crud 
        (bool Success, String Message) AddProduct(Product product);
        (bool Success, String Message) UpdateProduct(Product product);
        (bool Success, String Message) DeleteProduct(int id);

        Product? GetById(int id);
        IEnumerable<Product> GetAll();

        // LINQ Queries
        IEnumerable<Product> GetByCategory(string category);
        IEnumerable<Product> GetByPriceRange(decimal min, decimal max);
        IEnumerable<Product> GetSortedByPrice(bool ascending = true);
        IEnumerable<Product> GetTopNMostExpensive(int n = 5);

        // Business Logic Using configuration
        decimal GetTotalInventoryValue();
        decimal CalculatePriceWithTax(decimal price);
        decimal CalculateDiscountedPrice(decimal price);
        IEnumerable<Product> GetLowStockProducts();

    }
}

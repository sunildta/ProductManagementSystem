using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; }

        /*public Product(int id, string name, decimal price, string category, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Category = category;
            Stock = stock;
        }
        */
        public override string ToString()
        {
            return $"[{Id}] {Name} | {Category} | ${Price:F2} | Stock: {Stock}";
        }
    }
}
using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Data
{
    public static class DataSeeder
    {
        public static void Seed(IProductRepository repo)
        {
            if (repo.GetAll().Any())
            {
                return; // Data already seeded
            }
            var products = new[]
            {
            new Product { Name = "MacBook Pro 16\"",      Price = 2499.99m, Category = "Electronics",  Stock = 15  },
            new Product { Name = "Dell XPS 15",           Price = 1799.99m, Category = "Electronics",  Stock = 20  },
            new Product { Name = "Sony WH-1000XM5",       Price =  349.99m, Category = "Electronics",  Stock = 80  },
            new Product { Name = "iPad Air",               Price =  749.99m, Category = "Electronics",  Stock = 50  },
            new Product { Name = "Samsung 4K Monitor",    Price =  599.99m, Category = "Electronics",  Stock = 30  },
            new Product { Name = "Mechanical Keyboard",   Price =  129.99m, Category = "Accessories",  Stock = 120 },
            new Product { Name = "Logitech MX Master 3",  Price =   99.99m, Category = "Accessories",  Stock = 200 },
            new Product { Name = "USB-C Hub 7-in-1",      Price =   49.99m, Category = "Accessories",  Stock = 300 },
            new Product { Name = "Webcam 4K",             Price =  199.99m, Category = "Accessories",  Stock = 75  },
            new Product { Name = "Ergonomic Chair",       Price =  449.99m, Category = "Furniture",    Stock = 25  },
            new Product { Name = "Standing Desk",         Price =  699.99m, Category = "Furniture",    Stock = 18  },
            new Product { Name = "Monitor Arm",           Price =   89.99m, Category = "Furniture",    Stock = 60  },
            new Product { Name = "Clean Code (Book)",     Price =   39.99m, Category = "Books",        Stock = 500 },
            new Product { Name = "C# in Depth",           Price =   49.99m, Category = "Books",        Stock = 350 },
            new Product { Name = "Designing Data-Intens.", Price =  54.99m, Category = "Books",        Stock = 280 },
        };

            foreach (var p in products)
                repo.Add(p);
        }
    }
}

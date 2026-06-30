using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Data
{
    public static class DataSeeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            // Already seeded — skip
            if (db.Categories.Any()) return;

            // ── Categories ────────────────────────────────────────────────────────
            var electronics = new Category { Name = "Electronics", Description = "Electronic devices and gadgets" };
            var accessories = new Category { Name = "Accessories", Description = "Computer peripherals and add-ons" };
            var furniture = new Category { Name = "Furniture", Description = "Office and home furniture" };
            var books = new Category { Name = "Books", Description = "Technical and programming books" };

            db.Categories.AddRange(electronics, accessories, furniture, books);

            // ── Suppliers ─────────────────────────────────────────────────────────
            var appleSupplier = new Supplier { Name = "Apple Inc.", Email = "supply@apple.com", Phone = "+1-800-275-2273" };
            var dellSupplier = new Supplier { Name = "Dell Technologies", Email = "supply@dell.com", Phone = "+1-800-624-9897" };
            var logitech = new Supplier { Name = "Logitech", Email = "supply@logitech.com", Phone = "+41-21-863-5111" };
            var ikea = new Supplier { Name = "IKEA", Email = "supply@ikea.com", Phone = "+46-8-553-555-00" };
            var oreilly = new Supplier { Name = "O'Reilly Media", Email = "supply@oreilly.com", Phone = "+1-707-827-7000" };

            db.Suppliers.AddRange(appleSupplier, dellSupplier, logitech, ikea, oreilly);

            // ── Products ──────────────────────────────────────────────────────────
            var products = new List<Product>
        {
            new() { Name = "MacBook Pro 16\"",       Price = 2499.99m, Stock = 15,  Category = electronics, Supplier = appleSupplier },
            new() { Name = "iPad Air",               Price =  749.99m, Stock = 50,  Category = electronics, Supplier = appleSupplier },
            new() { Name = "Dell XPS 15",            Price = 1799.99m, Stock = 20,  Category = electronics, Supplier = dellSupplier  },
            new() { Name = "Samsung 4K Monitor",     Price =  599.99m, Stock = 30,  Category = electronics, Supplier = dellSupplier  },
            new() { Name = "Logitech MX Master 3",   Price =   99.99m, Stock = 200, Category = accessories, Supplier = logitech      },
            new() { Name = "Mechanical Keyboard",    Price =  129.99m, Stock = 120, Category = accessories, Supplier = logitech      },
            new() { Name = "Webcam 4K",              Price =  199.99m, Stock = 75,  Category = accessories, Supplier = logitech      },
            new() { Name = "USB-C Hub 7-in-1",       Price =   49.99m, Stock = 300, Category = accessories, Supplier = logitech      },
            new() { Name = "Ergonomic Chair",        Price =  449.99m, Stock = 25,  Category = furniture,   Supplier = ikea          },
            new() { Name = "Standing Desk",          Price =  699.99m, Stock = 18,  Category = furniture,   Supplier = ikea          },
            new() { Name = "Monitor Arm",            Price =   89.99m, Stock = 60,  Category = furniture,   Supplier = ikea          },
            new() { Name = "Clean Code",             Price =   39.99m, Stock = 500, Category = books,       Supplier = oreilly       },
            new() { Name = "C# in Depth",            Price =   49.99m, Stock = 350, Category = books,       Supplier = oreilly       },
            new() { Name = "Designing Data-Intensive Apps", Price = 54.99m, Stock = 280, Category = books,  Supplier = oreilly       },
        };

            db.Products.AddRange(products);

            // ── Reviews ───────────────────────────────────────────────────────────
            db.ProductReviews.AddRange(
                new ProductReview { Product = products[0], Rating = 5, Comment = "Best laptop I've ever used!" },
                new ProductReview { Product = products[0], Rating = 4, Comment = "Pricey but worth it." },
                new ProductReview { Product = products[4], Rating = 5, Comment = "Incredibly smooth mouse." },
                new ProductReview { Product = products[11], Rating = 5, Comment = "Must read for every developer." }
            );

            db.SaveChanges();
        }
    }
}

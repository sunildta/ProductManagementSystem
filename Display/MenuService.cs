using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Display
{
    public class MenuService(IProductRepository repo)

    {
        // ── Main Loop ─────────────────────────────────────────────────────────────
        public void Run()
        {
            while (true)
            {
                ShowMainMenu();
                string choice = Prompt("Enter choice");

                switch (choice)
                {
                    case "1": HandleViewProducts(); break;
                    case "2": HandleAddProduct(); break;
                    case "3": HandleUpdateProduct(); break;
                    case "4": HandleDeleteProduct(); break;
                    case "5": HandleSearch(); break;
                    case "6": HandleReports(); break;
                    case "0":
                        DisplayService.Header("Goodbye!");
                        return;
                    default:
                        DisplayService.Error("Invalid option. Try again.");
                        Pause();
                        break;
                }
            }
        }

        // ── Main Menu ─────────────────────────────────────────────────────────────
        private static void ShowMainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║        PRODUCT MANAGER  v1.0         ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.ResetColor();
            Console.WriteLine("║  1. View Products                    ║");
            Console.WriteLine("║  2. Add Product                      ║");
            Console.WriteLine("║  3. Update Product                   ║");
            Console.WriteLine("║  4. Delete Product                   ║");
            Console.WriteLine("║  5. Search / Filter                  ║");
            Console.WriteLine("║  6. Reports                          ║");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("║──────────────────────────────────────║");
            Console.ResetColor();
            Console.WriteLine("║  0. Exit                             ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
        }

        // ══════════════════════════════════════════════════════════════════════════
        //  1. VIEW PRODUCTS
        // ══════════════════════════════════════════════════════════════════════════
        private void HandleViewProducts()
        {
            while (true)
            {
                DisplayService.Header("View Products");
                Console.WriteLine("  1. All products");
                Console.WriteLine("  2. Find by ID");
                Console.WriteLine("  0. Back");

                switch (Prompt("Choice"))
                {
                    case "1":
                        DisplayService.Header("All Products");
                        DisplayService.PrintProducts(repo.GetAll());
                        Pause();
                        break;
                    case "2":
                        DisplayService.Header("Find by ID");
                        if (!int.TryParse(Prompt("Enter product ID"), out int id))
                        {
                            DisplayService.Error("Invalid ID.");
                        }
                        else
                        {
                            var p = repo.GetById(id);
                            if (p is null) DisplayService.Error($"No product with ID {id}.");
                            else DisplayService.PrintProducts([p]);
                        }
                        Pause();
                        break;
                    case "0": return;
                    default:
                        DisplayService.Error("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════════════
        //  2. ADD PRODUCT
        // ══════════════════════════════════════════════════════════════════════════
        private void HandleAddProduct()
        {
            DisplayService.Header("Add New Product");

            string name = Prompt("Name");
            string category = Prompt("Category");

            if (!decimal.TryParse(Prompt("Price"), out decimal price) || price < 0)
            {
                DisplayService.Error("Invalid price. Operation cancelled.");
                Pause(); return;
            }
            if (!int.TryParse(Prompt("Stock"), out int stock) || stock < 0)
            {
                DisplayService.Error("Invalid stock. Operation cancelled.");
                Pause(); return;
            }

            var product = new Product { Name = name, Category = category, Price = price, Stock = stock };
            repo.Add(product);
            DisplayService.Success($"Product added → {product}");
            Pause();
        }

        // ══════════════════════════════════════════════════════════════════════════
        //  3. UPDATE PRODUCT
        // ══════════════════════════════════════════════════════════════════════════
        private void HandleUpdateProduct()
        {
            DisplayService.Header("Update Product");
            DisplayService.PrintProducts(repo.GetAll());

            if (!int.TryParse(Prompt("Enter ID to update"), out int id))
            {
                DisplayService.Error("Invalid ID."); Pause(); return;
            }

            var existing = repo.GetById(id);
            if (existing is null)
            {
                DisplayService.Error($"Product ID {id} not found."); Pause(); return;
            }

            Console.WriteLine($"\n  Editing: {existing}");
            Console.WriteLine("  (Press Enter to keep current value)\n");

            string nameInput = Prompt($"Name [{existing.Name}]");
            if (!string.IsNullOrWhiteSpace(nameInput)) existing.Name = nameInput;

            string catInput = Prompt($"Category [{existing.Category}]");
            if (!string.IsNullOrWhiteSpace(catInput)) existing.Category = catInput;

            string priceInput = Prompt($"Price [{existing.Price:F2}]");
            if (!string.IsNullOrWhiteSpace(priceInput))
            {
                if (decimal.TryParse(priceInput, out decimal newPrice) && newPrice >= 0)
                    existing.Price = newPrice;
                else { DisplayService.Error("Invalid price — keeping old value."); }
            }

            string stockInput = Prompt($"Stock [{existing.Stock}]");
            if (!string.IsNullOrWhiteSpace(stockInput))
            {
                if (int.TryParse(stockInput, out int newStock) && newStock >= 0)
                    existing.Stock = newStock;
                else { DisplayService.Error("Invalid stock — keeping old value."); }
            }

            repo.Update(existing);
            DisplayService.Success($"Updated → {existing}");
            Pause();
        }

        // ══════════════════════════════════════════════════════════════════════════
        //  4. DELETE PRODUCT
        // ══════════════════════════════════════════════════════════════════════════
        private void HandleDeleteProduct()
        {
            DisplayService.Header("Delete Product");
            DisplayService.PrintProducts(repo.GetAll());

            if (!int.TryParse(Prompt("Enter ID to delete"), out int id))
            {
                DisplayService.Error("Invalid ID."); Pause(); return;
            }

            var product = repo.GetById(id);
            if (product is null)
            {
                DisplayService.Error($"Product ID {id} not found."); Pause(); return;
            }

            Console.Write($"\n  Delete \"{product.Name}\"? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() != "y")
            {
                Console.WriteLine("  Cancelled."); Pause(); return;
            }

            repo.Delete(id);
            DisplayService.Success($"Product ID {id} deleted.");
            Pause();
        }

        // ══════════════════════════════════════════════════════════════════════════
        //  5. SEARCH / FILTER
        // ══════════════════════════════════════════════════════════════════════════
        private void HandleSearch()
        {
            while (true)
            {
                DisplayService.Header("Search / Filter");
                Console.WriteLine("  1. By Category");
                Console.WriteLine("  2. By Price Range");
                Console.WriteLine("  3. Sort by Price (Ascending)");
                Console.WriteLine("  4. Sort by Price (Descending)");
                Console.WriteLine("  0. Back");

                switch (Prompt("Choice"))
                {
                    case "1":
                        DisplayService.Header("Filter by Category");
                        string cat = Prompt("Enter category");
                        DisplayService.PrintProducts(
                            repo.GetByCategory(cat),
                            $"No products found in category '{cat}'.");
                        Pause();
                        break;

                    case "2":
                        DisplayService.Header("Filter by Price Range");
                        if (!decimal.TryParse(Prompt("Min price"), out decimal min) ||
                            !decimal.TryParse(Prompt("Max price"), out decimal max))
                        {
                            DisplayService.Error("Invalid price values.");
                        }
                        else
                        {
                            DisplayService.PrintProducts(
                                repo.GetByPriceRange(min, max),
                                $"No products between ${min:F2} and ${max:F2}.");
                        }
                        Pause();
                        break;

                    case "3":
                        DisplayService.Header("Sorted by Price – Low to High");
                        DisplayService.PrintProducts(repo.GetSortedByPrice(ascending: true));
                        Pause();
                        break;

                    case "4":
                        DisplayService.Header("Sorted by Price – High to Low");
                        DisplayService.PrintProducts(repo.GetSortedByPrice(ascending: false));
                        Pause();
                        break;

                    case "0": return;

                    default:
                        DisplayService.Error("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════════════
        //  6. REPORTS
        // ══════════════════════════════════════════════════════════════════════════
        private void HandleReports()
        {
            while (true)
            {
                DisplayService.Header("Reports");
                Console.WriteLine("  1. Total Inventory Value");
                Console.WriteLine("  2. Top 5 Most Expensive Products");
                Console.WriteLine("  3. Stock Summary by Category");
                Console.WriteLine("  0. Back");

                switch (Prompt("Choice"))
                {
                    case "1":
                        DisplayService.Header("Total Inventory Value");
                        DisplayService.PrintValue("Total Value (Price × Stock)", repo.GetTotalInventoryValue());
                        Pause();
                        break;

                    case "2":
                        DisplayService.Header("Top 5 Most Expensive Products");
                        DisplayService.PrintProducts(repo.GetTopNMostExpensive(5));
                        Pause();
                        break;

                    case "3":
                        DisplayService.Header("Stock Summary by Category");
                        var summary = repo.GetAll()
                            .GroupBy(p => p.Category)
                            .Select(g => new
                            {
                                Category = g.Key,
                                Count = g.Count(),
                                Total = g.Sum(p => p.Stock),
                                Value = g.Sum(p => p.Price * p.Stock)
                            })
                            .OrderBy(g => g.Category);

                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"  {"Category",-16} | {"Products",8} | {"Stock",8} | {"Value",12}");
                        Console.WriteLine($"  {new string('-', 54)}");
                        Console.ResetColor();
                        foreach (var row in summary)
                            Console.WriteLine($"  {row.Category,-16} | {row.Count,8} | {row.Total,8} | ${row.Value,11:N2}");
                        Pause();
                        break;

                    case "0": return;

                    default:
                        DisplayService.Error("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────────
        private static string Prompt(string label)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"\n  {label}: ");
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        private static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\n  Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey(intercept: true);
        }
    }
}

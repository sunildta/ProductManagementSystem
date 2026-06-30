using ProductManagementSystem.IOptions;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;
using ProductManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.DisplayServices;
/// <summary>
/// Top-level router only. Each entity's menu logic lives in its own
/// *MenuHandler class under Menus/ — this class just wires them together
/// and owns the main loop.
/// </summary>
public class MenuService(
    ProductMenuHandler productMenu,
    CategoryMenuHandler categoryMenu,
    SupplierMenuHandler supplierMenu,
    ReviewMenuHandler reviewMenu)
{
    public void Run() => RunAsync().GetAwaiter().GetResult();

    private async Task RunAsync()
    {
        while (true)
        {
            ShowMainMenu();
            switch (MenuHelpers.Prompt("Enter choice"))
            {
                case "1": await productMenu.ShowAsync(); break;
                case "2": await categoryMenu.ShowAsync(); break;
                case "3": await supplierMenu.ShowAsync(); break;
                case "4": await reviewMenu.ShowAsync(); break;
                case "0":
                    DisplayService.Header("Goodbye!");
                    return;
                default:
                    DisplayService.Error("Invalid option. Try again.");
                    MenuHelpers.Pause();
                    break;
            }
        }
    }

    private static void ShowMainMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║        PRODUCT MANAGER  v5.0         ║");
        Console.WriteLine("╠══════════════════════════════════════╣");
        Console.ResetColor();
        Console.WriteLine("║  1. Products                         ║");
        Console.WriteLine("║  2. Categories                        ║");
        Console.WriteLine("║  3. Suppliers                         ║");
        Console.WriteLine("║  4. Reviews                           ║");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("║──────────────────────────────────────║");
        Console.ResetColor();
        Console.WriteLine("║  0. Exit                             ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
    }
}

/*{
    // CONSTRUCTOR INJECTION: Handing the dependency to the class when it is created.
    /// <summary>
    /// Interactive console menu.
    /// Now depends on IProductService (business layer) instead of IProductRepository directly.
    /// DI container injects it via constructor.
    /// </summary>
    public class MenuService(IProductService svc)
    {
        // ── Main Loop ─────────────────────────────────────────────────────────────
        public void Run()
        {
            while (true)
            {
                ShowMainMenu();
                switch (Prompt("Enter choice"))
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
            Console.WriteLine("║        PRODUCT MANAGER  v2.0         ║");
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
                        DisplayService.PrintProducts(svc.GetAll());
                        Pause(); break;
                    case "2":
                        DisplayService.Header("Find by ID");
                        if (!int.TryParse(Prompt("Enter product ID"), out int id))
                            DisplayService.Error("Invalid ID.");
                        else
                        {
                            var p = svc.GetById(id);
                            if (p is null) DisplayService.Error($"No product with ID {id}.");
                            else
                            {
                                DisplayService.PrintProducts([p]);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"\n  Price + Tax   : ${svc.CalculatePriceWithTax(p.Price):F2}");
                                Console.WriteLine($"  Price - Disc. : ${svc.CalculateDiscountedPrice(p.Price):F2}");
                                Console.ResetColor();
                            }
                        }
                        Pause(); break;
                    case "0": return;
                    default:
                        DisplayService.Error("Invalid option."); Pause(); break;
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

            if (!decimal.TryParse(Prompt("Price"), out decimal price))
            {
                DisplayService.Error("Invalid price."); Pause(); return;
            }
            if (!int.TryParse(Prompt("Stock"), out int stock))
            {
                DisplayService.Error("Invalid stock."); Pause(); return;
            }

            var product = new Product { Name = name, Category = new Category { Name = category }, Price = price, Stock = stock };
            var (success, message) = svc.AddProduct(product);

            if (success)
            {
                DisplayService.Success(message);
                Console.WriteLine($"\n  Base Price    : ${price:F2}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  Price + Tax   : ${svc.CalculatePriceWithTax(price):F2}");
                Console.WriteLine($"  Price - Disc. : ${svc.CalculateDiscountedPrice(price):F2}");
                Console.ResetColor();
            }
            else
                DisplayService.Error(message);
            Pause();
        }

        // ══════════════════════════════════════════════════════════════════════════
        //  3. UPDATE PRODUCT
        // ══════════════════════════════════════════════════════════════════════════
        private void HandleUpdateProduct()
        {
            DisplayService.Header("Update Product");
            DisplayService.PrintProducts(svc.GetAll());

            if (!int.TryParse(Prompt("Enter ID to update"), out int id))
            {
                DisplayService.Error("Invalid ID."); Pause(); return;
            }

            var existing = svc.GetById(id);
            if (existing is null)
            {
                DisplayService.Error($"Product ID {id} not found."); Pause(); return;
            }

            Console.WriteLine($"\n  Editing: {existing}");
            Console.WriteLine("  (Press Enter to keep current value)\n");

            string nameInput = Prompt($"Name [{existing.Name}]");
            if (!string.IsNullOrWhiteSpace(nameInput)) existing.Name = nameInput;

            string catInput = Prompt($"Category [{existing.Category?.Name}]");
            if (!string.IsNullOrWhiteSpace(catInput)) existing.Category = new Category { Name = catInput };

            string priceInput = Prompt($"Price [{existing.Price:F2}]");
            if (!string.IsNullOrWhiteSpace(priceInput) &&
                decimal.TryParse(priceInput, out decimal newPrice))
                existing.Price = newPrice;

            string stockInput = Prompt($"Stock [{existing.Stock}]");
            if (!string.IsNullOrWhiteSpace(stockInput) &&
                int.TryParse(stockInput, out int newStock))
                existing.Stock = newStock;

            var (success, message) = svc.UpdateProduct(existing);
            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
            Pause();
        }

        // ══════════════════════════════════════════════════════════════════════════
        //  4. DELETE PRODUCT
        // ══════════════════════════════════════════════════════════════════════════
        private void HandleDeleteProduct()
        {
            DisplayService.Header("Delete Product");
            DisplayService.PrintProducts(svc.GetAll());

            if (!int.TryParse(Prompt("Enter ID to delete"), out int id))
            {
                DisplayService.Error("Invalid ID."); Pause(); return;
            }

            var product = svc.GetById(id);
            if (product is null)
            {
                DisplayService.Error($"Product ID {id} not found."); Pause(); return;
            }

            Console.Write($"\n  Delete \"{product.Name}\"? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() != "y")
            {
                Console.WriteLine("  Cancelled."); Pause(); return;
            }

            var (success, message) = svc.DeleteProduct(id);
            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
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
                        DisplayService.PrintProducts(svc.GetByCategory(cat),
                            $"No products found in category '{cat}'.");
                        Pause(); break;
                    case "2":
                        DisplayService.Header("Filter by Price Range");
                        if (!decimal.TryParse(Prompt("Min price"), out decimal min) ||
                            !decimal.TryParse(Prompt("Max price"), out decimal max))
                            DisplayService.Error("Invalid price values.");
                        else
                            DisplayService.PrintProducts(svc.GetByPriceRange(min, max),
                                $"No products between ${min:F2} and ${max:F2}.");
                        Pause(); break;
                    case "3":
                        DisplayService.Header("Sorted by Price – Low to High");
                        DisplayService.PrintProducts(svc.GetSortedByPrice(ascending: true));
                        Pause(); break;
                    case "4":
                        DisplayService.Header("Sorted by Price – High to Low");
                        DisplayService.PrintProducts(svc.GetSortedByPrice(ascending: false));
                        Pause(); break;
                    case "0": return;
                    default:
                        DisplayService.Error("Invalid option."); Pause(); break;
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
                Console.WriteLine("  4. Low Stock Alert");
                Console.WriteLine("  5. Price with Tax & Discount (demo)");
                Console.WriteLine("  0. Back");

                switch (Prompt("Choice"))
                {
                    case "1":
                        DisplayService.Header("Total Inventory Value");
                        DisplayService.PrintValue("Total Value (Price × Stock)", svc.GetTotalInventoryValue());
                        Pause(); break;

                    case "2":
                        DisplayService.Header("Top 5 Most Expensive Products");
                        DisplayService.PrintProducts(svc.GetTopNMostExpensive(5));
                        Pause(); break;

                    case "3":
                        DisplayService.Header("Stock Summary by Category");
                        var summary = svc.GetAll()
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
                        Pause(); break;

                    case "4":
                        DisplayService.Header("Low Stock Alert");
                        DisplayService.PrintProducts(svc.GetLowStockProducts(), "No low-stock products.");
                        Pause(); break;

                    case "5":
                        DisplayService.Header("Price Calculator (Tax & Discount)");
                        if (!decimal.TryParse(Prompt("Enter base price"), out decimal basePrice))
                        {
                            DisplayService.Error("Invalid price."); Pause(); break;
                        }
                        decimal withTax = svc.CalculatePriceWithTax(basePrice);
                        decimal withDiscount = svc.CalculateDiscountedPrice(basePrice);
                        Console.WriteLine($"\n  Base Price      : ${basePrice:F2}");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"  With Tax        : ${withTax:F2}");
                        Console.WriteLine($"  With Discount   : ${withDiscount:F2}");
                        Console.ResetColor();
                        Pause(); break;

                    case "0": return;
                    default:
                        DisplayService.Error("Invalid option."); Pause(); break;
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
}*/



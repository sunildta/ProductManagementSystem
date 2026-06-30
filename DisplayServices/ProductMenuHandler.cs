using ProductManagementSystem.IOptions;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;

namespace ProductManagementSystem.DisplayServices
{
    /// <summary>
    /// Handles the Product sub-menu.
    /// Injected with IProductService for business logic.
    /// </summary>
    public class ProductMenuHandler
    {
        private readonly IProductService _svc;
        public ProductMenuHandler(IProductService svc) => _svc = svc;

        public async Task ShowAsync()
        {
            while (true)
            {
                DisplayService.Header("Products");
                Console.WriteLine("  1. View All Products");
                Console.WriteLine("  2. Find by ID");
                Console.WriteLine("  3. Add Product");
                Console.WriteLine("  4. Update Product");
                Console.WriteLine("  5. Delete Product");
                Console.WriteLine("  6. Search / Filter");
                Console.WriteLine("  7. Reports");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  ──────────────────────");
                Console.ResetColor();
                Console.WriteLine("  0. Back");

                switch (MenuHelpers.Prompt("Enter choice"))
                {
                    case "1": await HandleViewAllAsync(); break;
                    case "2": await HandleFindByIdAsync(); break;
                    case "3": await HandleAddAsync(); break;
                    case "4": await HandleUpdateAsync(); break;
                    case "5": await HandleDeleteAsync(); break;
                    case "6": await HandleSearchAsync(); break;
                    case "7": await HandleReportsAsync(); break;
                    case "0": return;
                    default:
                        DisplayService.Error("Invalid option. Try again.");
                        MenuHelpers.Pause();
                        break;
                }
            }
        }

        private async Task HandleViewAllAsync()
        {
            DisplayService.Header("All Products");
            DisplayService.PrintProducts(await _svc.GetAllAsync());
            MenuHelpers.Pause();
        }

        private async Task HandleFindByIdAsync()
        {
            DisplayService.Header("Find by ID");
            if (!int.TryParse(MenuHelpers.Prompt("Enter product ID"), out int id))
            {
                DisplayService.Error("Invalid ID.");
                MenuHelpers.Pause();
                return;
            }

            var p = await _svc.GetByIdAsync(id);
            if (p is null)
            {
                DisplayService.Error($"No product with ID {id}.");
            }
            else
            {
                DisplayService.PrintProducts([p]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n  Price + Tax   : ${_svc.CalculatePriceWithTax(p.Price):F2}");
                Console.WriteLine($"  Price - Disc. : ${_svc.CalculateDiscountedPrice(p.Price):F2}");
                Console.ResetColor();
            }
            MenuHelpers.Pause();
        }

        private async Task HandleAddAsync()
        {
            DisplayService.Header("Add New Product");

            // Show available categories
            var categories = await _svc.GetAllAsync();

            string name = MenuHelpers.Prompt("Name");
            if (!decimal.TryParse(MenuHelpers.Prompt("Price"), out decimal price))
            {
                DisplayService.Error("Invalid price.");
                MenuHelpers.Pause();
                return;
            }
            if (!int.TryParse(MenuHelpers.Prompt("Stock"), out int stock))
            {
                DisplayService.Error("Invalid stock.");
                MenuHelpers.Pause();
                return;
            }
            if (!int.TryParse(MenuHelpers.Prompt("Category ID"), out int categoryId))
            {
                DisplayService.Error("Invalid category ID.");
                MenuHelpers.Pause();
                return;
            }
            if (!int.TryParse(MenuHelpers.Prompt("Supplier ID"), out int supplierId))
            {
                DisplayService.Error("Invalid supplier ID.");
                MenuHelpers.Pause();
                return;
            }

            var product = new Product
            {
                Name = name,
                Price = price,
                Stock = stock,
                CategoryId = categoryId,
                SupplierId = supplierId
            };

            var (success, message) = await _svc.AddProductAsync(product);
            if (success)
            {
                DisplayService.Success(message);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n  Price + Tax   : ${_svc.CalculatePriceWithTax(price):F2}");
                Console.WriteLine($"  Price - Disc. : ${_svc.CalculateDiscountedPrice(price):F2}");
                Console.ResetColor();
            }
            else
                DisplayService.Error(message);

            MenuHelpers.Pause();
        }

        private async Task HandleUpdateAsync()
        {
            DisplayService.Header("Update Product");
            DisplayService.PrintProducts(await _svc.GetAllAsync());

            if (!int.TryParse(MenuHelpers.Prompt("Enter ID to update"), out int id))
            {
                DisplayService.Error("Invalid ID.");
                MenuHelpers.Pause();
                return;
            }

            var existing = await _svc.GetByIdAsync(id);
            if (existing is null)
            {
                DisplayService.Error($"Product ID {id} not found.");
                MenuHelpers.Pause();
                return;
            }

            Console.WriteLine($"\n  Editing: {existing}");
            Console.WriteLine("  (Press Enter to keep current value)\n");

            string nameInput = MenuHelpers.Prompt($"Name [{existing.Name}]");
            if (!string.IsNullOrWhiteSpace(nameInput)) existing.Name = nameInput;

            string priceInput = MenuHelpers.Prompt($"Price [{existing.Price:F2}]");
            if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out decimal newPrice))
                existing.Price = newPrice;

            string stockInput = MenuHelpers.Prompt($"Stock [{existing.Stock}]");
            if (!string.IsNullOrWhiteSpace(stockInput) && int.TryParse(stockInput, out int newStock))
                existing.Stock = newStock;

            var (success, message) = await _svc.UpdateProductAsync(existing);
            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
            MenuHelpers.Pause();
        }

        private async Task HandleDeleteAsync()
        {
            DisplayService.Header("Delete Product");
            DisplayService.PrintProducts(await _svc.GetAllAsync());

            if (!int.TryParse(MenuHelpers.Prompt("Enter ID to delete"), out int id))
            {
                DisplayService.Error("Invalid ID.");
                MenuHelpers.Pause();
                return;
            }

            var product = await _svc.GetByIdAsync(id);
            if (product is null)
            {
                DisplayService.Error($"Product ID {id} not found.");
                MenuHelpers.Pause();
                return;
            }

            Console.Write($"\n  Delete \"{product.Name}\"? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() != "y")
            {
                Console.WriteLine("  Cancelled.");
                MenuHelpers.Pause();
                return;
            }

            var (success, message) = await _svc.DeleteProductAsync(id);
            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
            MenuHelpers.Pause();
        }

        private async Task HandleSearchAsync()
        {
            while (true)
            {
                DisplayService.Header("Search / Filter");
                Console.WriteLine("  1. By Category");
                Console.WriteLine("  2. By Price Range");
                Console.WriteLine("  3. Sort by Price (Ascending)");
                Console.WriteLine("  4. Sort by Price (Descending)");
                Console.WriteLine("  0. Back");

                switch (MenuHelpers.Prompt("Choice"))
                {
                    case "1":
                        DisplayService.Header("Filter by Category");
                        string cat = MenuHelpers.Prompt("Enter category");
                        DisplayService.PrintProducts(await _svc.GetByCategoryAsync(cat),
                            $"No products found in category '{cat}'.");
                        MenuHelpers.Pause(); break;
                    case "2":
                        DisplayService.Header("Filter by Price Range");
                        if (!decimal.TryParse(MenuHelpers.Prompt("Min price"), out decimal min) ||
                            !decimal.TryParse(MenuHelpers.Prompt("Max price"), out decimal max))
                            DisplayService.Error("Invalid price values.");
                        else
                            DisplayService.PrintProducts(await _svc.GetByPriceRangeAsync(min, max),
                                $"No products between ${min:F2} and ${max:F2}.");
                        MenuHelpers.Pause(); break;
                    case "3":
                        DisplayService.Header("Sorted by Price – Low to High");
                        DisplayService.PrintProducts(await _svc.GetSortedByPriceAsync(ascending: true));
                        MenuHelpers.Pause(); break;
                    case "4":
                        DisplayService.Header("Sorted by Price – High to Low");
                        DisplayService.PrintProducts(await _svc.GetSortedByPriceAsync(ascending: false));
                        MenuHelpers.Pause(); break;
                    case "0": return;
                    default:
                        DisplayService.Error("Invalid option.");
                        MenuHelpers.Pause(); break;
                }
            }
        }

        private async Task HandleReportsAsync()
        {
            while (true)
            {
                DisplayService.Header("Reports");
                Console.WriteLine("  1. Total Inventory Value");
                Console.WriteLine("  2. Top 5 Most Expensive Products");
                Console.WriteLine("  3. Low Stock Alert");
                Console.WriteLine("  4. Price with Tax & Discount (demo)");
                Console.WriteLine("  0. Back");

                switch (MenuHelpers.Prompt("Choice"))
                {
                    case "1":
                        DisplayService.Header("Total Inventory Value");
                        DisplayService.PrintValue("Total Value (Price × Stock)",
                            await _svc.GetTotalInventoryValueAsync());
                        MenuHelpers.Pause(); break;
                    case "2":
                        DisplayService.Header("Top 5 Most Expensive Products");
                        DisplayService.PrintProducts(await _svc.GetTopNMostExpensiveAsync(5));
                        MenuHelpers.Pause(); break;
                    case "3":
                        DisplayService.Header("Low Stock Alert");
                        DisplayService.PrintProducts(await _svc.GetLowStockProductsAsync(),
                            "No low-stock products.");
                        MenuHelpers.Pause(); break;
                    case "4":
                        DisplayService.Header("Price Calculator (Tax & Discount)");
                        if (!decimal.TryParse(MenuHelpers.Prompt("Enter base price"), out decimal basePrice))
                        {
                            DisplayService.Error("Invalid price.");
                            MenuHelpers.Pause(); break;
                        }
                        decimal withTax = _svc.CalculatePriceWithTax(basePrice);
                        decimal withDiscount = _svc.CalculateDiscountedPrice(basePrice);
                        Console.WriteLine($"\n  Base Price      : ${basePrice:F2}");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"  With Tax        : ${withTax:F2}");
                        Console.WriteLine($"  With Discount   : ${withDiscount:F2}");
                        Console.ResetColor();
                        MenuHelpers.Pause(); break;
                    case "0": return;
                    default:
                        DisplayService.Error("Invalid option.");
                        MenuHelpers.Pause(); break;
                }
            }
        }
    }
}

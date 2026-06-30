using ProductManagementSystem.IOptions;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;

namespace ProductManagementSystem.DisplayServices
{
    /// <summary>
    /// Handles the Category sub-menu.
    /// Injected with ICategoryService for business logic.
    /// </summary>
    public class CategoryMenuHandler
    {
        private readonly ICategoryService _svc;
        public CategoryMenuHandler(ICategoryService svc) => _svc = svc;

        public async Task ShowAsync()
        {
            while (true)
            {
                DisplayService.Header("Categories");
                Console.WriteLine("  1. View All Categories");
                Console.WriteLine("  2. Add Category");
                Console.WriteLine("  3. Update Category");
                Console.WriteLine("  4. Delete Category");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  ──────────────────────");
                Console.ResetColor();
                Console.WriteLine("  0. Back");

                switch (MenuHelpers.Prompt("Enter choice"))
                {
                    case "1": await HandleViewAllAsync(); break;
                    case "2": await HandleAddAsync(); break;
                    case "3": await HandleUpdateAsync(); break;
                    case "4": await HandleDeleteAsync(); break;
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
            DisplayService.Header("All Categories");
            var categories = await _svc.GetAllAsync();
            var list = categories.ToList();
            if (list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  No categories found.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  {"[ID]",-6} {"Name",-20} | {"Description"}");
                Console.WriteLine($"  {new string('-', 50)}");
                Console.ResetColor();
                foreach (var c in list)
                    Console.WriteLine($"  {c}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  {list.Count} category(ies).");
                Console.ResetColor();
            }
            MenuHelpers.Pause();
        }

        private async Task HandleAddAsync()
        {
            DisplayService.Header("Add New Category");
            string name = MenuHelpers.Prompt("Name");
            string desc = MenuHelpers.Prompt("Description");

            var category = new Category { Name = name, Description = desc };
            var (success, message) = await _svc.AddCategoryAsync(category);

            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
            MenuHelpers.Pause();
        }

        private async Task HandleUpdateAsync()
        {
            DisplayService.Header("Update Category");
            await HandleViewAllAsync();

            if (!int.TryParse(MenuHelpers.Prompt("Enter ID to update"), out int id))
            {
                DisplayService.Error("Invalid ID.");
                MenuHelpers.Pause();
                return;
            }

            var existing = await _svc.GetByIdAsync(id);
            if (existing is null)
            {
                DisplayService.Error($"Category ID {id} not found.");
                MenuHelpers.Pause();
                return;
            }

            Console.WriteLine($"\n  Editing: {existing}");
            Console.WriteLine("  (Press Enter to keep current value)\n");

            string nameInput = MenuHelpers.Prompt($"Name [{existing.Name}]");
            if (!string.IsNullOrWhiteSpace(nameInput)) existing.Name = nameInput;

            string descInput = MenuHelpers.Prompt($"Description [{existing.Description}]");
            if (!string.IsNullOrWhiteSpace(descInput)) existing.Description = descInput;

            var (success, message) = await _svc.UpdateCategoryAsync(existing);
            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
            MenuHelpers.Pause();
        }

        private async Task HandleDeleteAsync()
        {
            DisplayService.Header("Delete Category");
            await HandleViewAllAsync();

            if (!int.TryParse(MenuHelpers.Prompt("Enter ID to delete"), out int id))
            {
                DisplayService.Error("Invalid ID.");
                MenuHelpers.Pause();
                return;
            }

            var category = await _svc.GetByIdAsync(id);
            if (category is null)
            {
                DisplayService.Error($"Category ID {id} not found.");
                MenuHelpers.Pause();
                return;
            }

            Console.Write($"\n  Delete \"{category.Name}\"? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() != "y")
            {
                Console.WriteLine("  Cancelled.");
                MenuHelpers.Pause();
                return;
            }

            var (success, message) = await _svc.DeleteCategoryAsync(id);
            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
            MenuHelpers.Pause();
        }
    }
}

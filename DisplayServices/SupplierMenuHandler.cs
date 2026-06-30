using ProductManagementSystem.IOptions;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;

namespace ProductManagementSystem.DisplayServices
{
    /// <summary>
    /// Handles the Supplier sub-menu.
    /// Injected with ISupplierService for business logic.
    /// </summary>
    public class SupplierMenuHandler
    {
        private readonly ISupplierService _svc;
        public SupplierMenuHandler(ISupplierService svc) => _svc = svc;

        public async Task ShowAsync()
        {
            while (true)
            {
                DisplayService.Header("Suppliers");
                Console.WriteLine("  1. View All Suppliers");
                Console.WriteLine("  2. Add Supplier");
                Console.WriteLine("  3. Update Supplier");
                Console.WriteLine("  4. Delete Supplier");
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
            DisplayService.Header("All Suppliers");
            var suppliers = await _svc.GetAllAsync();
            var list = suppliers.ToList();
            if (list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  No suppliers found.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  {"[ID]",-6} {"Name",-20} | {"Email",-25} | {"Phone"}");
                Console.WriteLine($"  {new string('-', 60)}");
                Console.ResetColor();
                foreach (var s in list)
                    Console.WriteLine($"  {s}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  {list.Count} supplier(s).");
                Console.ResetColor();
            }
            MenuHelpers.Pause();
        }

        private async Task HandleAddAsync()
        {
            DisplayService.Header("Add New Supplier");
            string name = MenuHelpers.Prompt("Name");
            string email = MenuHelpers.Prompt("Email");
            string phone = MenuHelpers.Prompt("Phone");

            var supplier = new Supplier { Name = name, Email = email, Phone = phone };
            var (success, message) = await _svc.AddSupplierAsync(supplier);

            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
            MenuHelpers.Pause();
        }

        private async Task HandleUpdateAsync()
        {
            DisplayService.Header("Update Supplier");
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
                DisplayService.Error($"Supplier ID {id} not found.");
                MenuHelpers.Pause();
                return;
            }

            Console.WriteLine($"\n  Editing: {existing}");
            Console.WriteLine("  (Press Enter to keep current value)\n");

            string nameInput = MenuHelpers.Prompt($"Name [{existing.Name}]");
            if (!string.IsNullOrWhiteSpace(nameInput)) existing.Name = nameInput;

            string emailInput = MenuHelpers.Prompt($"Email [{existing.Email}]");
            if (!string.IsNullOrWhiteSpace(emailInput)) existing.Email = emailInput;

            string phoneInput = MenuHelpers.Prompt($"Phone [{existing.Phone}]");
            if (!string.IsNullOrWhiteSpace(phoneInput)) existing.Phone = phoneInput;

            var (success, message) = await _svc.UpdateSupplierAsync(existing);
            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
            MenuHelpers.Pause();
        }

        private async Task HandleDeleteAsync()
        {
            DisplayService.Header("Delete Supplier");
            await HandleViewAllAsync();

            if (!int.TryParse(MenuHelpers.Prompt("Enter ID to delete"), out int id))
            {
                DisplayService.Error("Invalid ID.");
                MenuHelpers.Pause();
                return;
            }

            var supplier = await _svc.GetByIdAsync(id);
            if (supplier is null)
            {
                DisplayService.Error($"Supplier ID {id} not found.");
                MenuHelpers.Pause();
                return;
            }

            Console.Write($"\n  Delete \"{supplier.Name}\"? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() != "y")
            {
                Console.WriteLine("  Cancelled.");
                MenuHelpers.Pause();
                return;
            }

            var (success, message) = await _svc.DeleteSupplierAsync(id);
            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
            MenuHelpers.Pause();
        }
    }
}

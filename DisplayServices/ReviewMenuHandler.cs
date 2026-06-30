using ProductManagementSystem.IOptions;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;

namespace ProductManagementSystem.DisplayServices
{
    /// <summary>
    /// Handles the Review sub-menu.
    /// Injected with IReviewService and IProductService for business logic.
    /// </summary>
    public class ReviewMenuHandler
    {
        private readonly IReviewService _svc;
        private readonly IProductService _productSvc;

        public ReviewMenuHandler(IReviewService svc, IProductService productSvc)
        {
            _svc = svc;
            _productSvc = productSvc;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                DisplayService.Header("Reviews");
                Console.WriteLine("  1. View All Reviews");
                Console.WriteLine("  2. View Reviews by Product");
                Console.WriteLine("  3. Add Review");
                Console.WriteLine("  4. Delete Review");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  ──────────────────────");
                Console.ResetColor();
                Console.WriteLine("  0. Back");

                switch (MenuHelpers.Prompt("Enter choice"))
                {
                    case "1": await HandleViewAllAsync(); break;
                    case "2": await HandleByProductAsync(); break;
                    case "3": await HandleAddAsync(); break;
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
            DisplayService.Header("All Reviews");
            var reviews = await _svc.GetAllAsync();
            PrintReviews(reviews);
            MenuHelpers.Pause();
        }

        private async Task HandleByProductAsync()
        {
            DisplayService.Header("Reviews by Product");
            if (!int.TryParse(MenuHelpers.Prompt("Enter Product ID"), out int productId))
            {
                DisplayService.Error("Invalid ID.");
                MenuHelpers.Pause();
                return;
            }

            var product = await _productSvc.GetByIdAsync(productId);
            if (product is null)
            {
                DisplayService.Error($"Product ID {productId} not found.");
                MenuHelpers.Pause();
                return;
            }

            Console.WriteLine($"\n  Product: {product.Name}");
            var reviews = await _svc.GetByProductIdAsync(productId);
            PrintReviews(reviews);
            MenuHelpers.Pause();
        }

        private async Task HandleAddAsync()
        {
            DisplayService.Header("Add New Review");

            if (!int.TryParse(MenuHelpers.Prompt("Product ID"), out int productId))
            {
                DisplayService.Error("Invalid product ID.");
                MenuHelpers.Pause();
                return;
            }

            var product = await _productSvc.GetByIdAsync(productId);
            if (product is null)
            {
                DisplayService.Error($"Product ID {productId} not found.");
                MenuHelpers.Pause();
                return;
            }

            if (!int.TryParse(MenuHelpers.Prompt("Rating (1-5)"), out int rating))
            {
                DisplayService.Error("Invalid rating.");
                MenuHelpers.Pause();
                return;
            }

            string comment = MenuHelpers.Prompt("Comment");

            var review = new ProductReview
            {
                ProductId = productId,
                Rating = rating,
                Comment = comment
            };

            var (success, message) = await _svc.AddReviewAsync(review);
            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
            MenuHelpers.Pause();
        }

        private async Task HandleDeleteAsync()
        {
            DisplayService.Header("Delete Review");
            await HandleViewAllAsync();

            if (!int.TryParse(MenuHelpers.Prompt("Enter Review ID to delete"), out int id))
            {
                DisplayService.Error("Invalid ID.");
                MenuHelpers.Pause();
                return;
            }

            Console.Write($"\n  Delete review ID {id}? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() != "y")
            {
                Console.WriteLine("  Cancelled.");
                MenuHelpers.Pause();
                return;
            }

            var (success, message) = await _svc.DeleteReviewAsync(id);
            if (success) DisplayService.Success(message);
            else DisplayService.Error(message);
            MenuHelpers.Pause();
        }

        private static void PrintReviews(IEnumerable<ProductReview> reviews)
        {
            var list = reviews.ToList();
            if (list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  No reviews found.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  {"[ID]",-6} {"Product",-20} | {"Rating",6} | {"Comment"}");
            Console.WriteLine($"  {new string('-', 60)}");
            Console.ResetColor();

            foreach (var r in list)
                Console.WriteLine($"  [{r.Id:D3}] {r.Product?.Name ?? "?",-20} | {r}");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  {list.Count} review(s).");
            Console.ResetColor();
        }
    }
}

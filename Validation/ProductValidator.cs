using ProductManagementSystem.Models;

namespace ProductManagementSystem.Validation
{
    /// <summary>
    /// Validates Product entities before persistence.
    /// Registered as Transient in DI.
    /// </summary>
    public class ProductValidator : IValidator<Product>
    {
        public FluentValidationResult Validate(Product product)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(product.Name))
                errors.Add("Product name is required.");
            else if (product.Name.Length > 100)
                errors.Add("Product name must be 100 characters or fewer.");

            if (product.Price < 0)
                errors.Add("Price must be zero or positive.");

            if (product.Stock < 0)
                errors.Add("Stock must be zero or positive.");

            if (product.CategoryId <= 0 && product.Category is null)
                errors.Add("A valid category is required.");

            if (product.SupplierId <= 0 && product.Supplier is null)
                errors.Add("A valid supplier is required.");

            return errors.Count > 0
                ? FluentValidationResult.WithErrors(errors.ToArray())
                : FluentValidationResult.Success();
        }
    }
}

using ProductManagementSystem.Models;

namespace ProductManagementSystem.Validation
{
    /// <summary>
    /// Validates Category entities before persistence.
    /// Registered as Transient in DI.
    /// </summary>
    public class CategoryValidator : IValidator<Category>
    {
        public FluentValidationResult Validate(Category category)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(category.Name))
                errors.Add("Category name is required.");
            else if (category.Name.Length > 50)
                errors.Add("Category name must be 50 characters or fewer.");

            if (category.Description?.Length > 100)
                errors.Add("Description must be 100 characters or fewer.");

            return errors.Count > 0
                ? FluentValidationResult.WithErrors(errors.ToArray())
                : FluentValidationResult.Success();
        }
    }
}

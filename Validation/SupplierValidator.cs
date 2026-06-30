using ProductManagementSystem.Models;

namespace ProductManagementSystem.Validation
{
    /// <summary>
    /// Validates Supplier entities before persistence.
    /// Registered as Transient in DI.
    /// </summary>
    public class SupplierValidator : IValidator<Supplier>
    {
        public FluentValidationResult Validate(Supplier supplier)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(supplier.Name))
                errors.Add("Supplier name is required.");
            else if (supplier.Name.Length > 50)
                errors.Add("Supplier name must be 50 characters or fewer.");

            if (string.IsNullOrWhiteSpace(supplier.Email))
                errors.Add("Supplier email is required.");
            else if (supplier.Email.Length > 75)
                errors.Add("Email must be 75 characters or fewer.");

            if (supplier.Phone?.Length > 50)
                errors.Add("Phone must be 50 characters or fewer.");

            return errors.Count > 0
                ? FluentValidationResult.WithErrors(errors.ToArray())
                : FluentValidationResult.Success();
        }
    }
}

using ProductManagementSystem.Models;

namespace ProductManagementSystem.Validation
{
    /// <summary>
    /// Validates ProductReview entities before persistence.
    /// Registered as Transient in DI.
    /// </summary>
    public class ProductReviewValidator : IValidator<ProductReview>
    {
        public FluentValidationResult Validate(ProductReview review)
        {
            var errors = new List<string>();

            if (review.Rating < 1 || review.Rating > 5)
                errors.Add("Rating must be between 1 and 5.");

            if (review.Comment?.Length > 1000)
                errors.Add("Comment must be 1000 characters or fewer.");

            if (review.ProductId <= 0 && review.Product is null)
                errors.Add("A valid product is required.");

            return errors.Count > 0
                ? FluentValidationResult.WithErrors(errors.ToArray())
                : FluentValidationResult.Success();
        }
    }
}

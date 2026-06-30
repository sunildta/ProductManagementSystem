using Microsoft.Extensions.Logging;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;
using ProductManagementSystem.Validation;

namespace ProductManagementSystem.Services
{
    /// <summary>
    /// Business logic layer for ProductReview management.
    /// Validates before writes, delegates to IProductReviewRepository.
    /// </summary>
    public class ReviewService : IReviewService
    {
        private readonly IProductReviewRepository _repo;
        private readonly IValidationService _validator;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(
            IProductReviewRepository repo,
            IValidationService validator,
            ILogger<ReviewService> logger)
        {
            _repo = repo;
            _validator = validator;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> AddReviewAsync(ProductReview review)
        {
            if (!_validator.Validate(review, out var errors))
            {
                string msg = string.Join(" | ", errors);
                _logger.LogWarning("AddReview validation failed: {Errors}", msg);
                return (false, msg);
            }

            await _repo.AddAsync(review);
            _logger.LogInformation("Review added: Id={Id} ProductId={ProductId}", review.Id, review.ProductId);
            return (true, $"Review added successfully (★{review.Rating}/5).");
        }

        public async Task<(bool Success, string Message)> UpdateReviewAsync(ProductReview review)
        {
            if (!_validator.Validate(review, out var errors))
                return (false, string.Join(" | ", errors));

            bool updated = await _repo.UpdateAsync(review);
            if (!updated) return (false, $"Review ID {review.Id} not found.");

            _logger.LogInformation("Review updated: Id={Id}", review.Id);
            return (true, $"Review ID {review.Id} updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteReviewAsync(int id)
        {
            bool deleted = await _repo.DeleteAsync(id);
            if (!deleted) return (false, $"Review ID {id} not found.");

            _logger.LogInformation("Review deleted: Id={Id}", id);
            return (true, $"Review ID {id} deleted.");
        }

        public Task<ProductReview?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<IEnumerable<ProductReview>> GetAllAsync() => _repo.GetAllAsync();
        public Task<IEnumerable<ProductReview>> GetByProductIdAsync(int productId) => _repo.GetByProductIdAsync(productId);
    }
}

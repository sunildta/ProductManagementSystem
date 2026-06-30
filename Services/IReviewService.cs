using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IReviewService
    {
        Task<(bool Success, string Message)> AddReviewAsync(ProductReview review);
        Task<(bool Success, string Message)> UpdateReviewAsync(ProductReview review);
        Task<(bool Success, string Message)> DeleteReviewAsync(int id);
        Task<ProductReview?> GetByIdAsync(int id);
        Task<IEnumerable<ProductReview>> GetAllAsync();
        Task<IEnumerable<ProductReview>> GetByProductIdAsync(int productId);
    }
}

using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repositories
{
    public interface IProductReviewRepository
    {
        Task AddAsync(ProductReview review);
        Task<ProductReview?> GetByIdAsync(int id);
        Task<IEnumerable<ProductReview>> GetAllAsync();
        Task<IEnumerable<ProductReview>> GetByProductIdAsync(int productId);
        Task<bool> UpdateAsync(ProductReview review);
        Task<bool> DeleteAsync(int id);
    }
}

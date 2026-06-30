using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repositories
{
    /// <summary>
    /// EF Core implementation of IProductReviewRepository.
    /// Scoped lifetime — shares DbContext within a DI scope.
    /// </summary>
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductReviewRepository(ApplicationDbContext db) => _db = db;

        public async Task AddAsync(ProductReview review)
        {
            await _db.ProductReviews.AddAsync(review);
            await _db.SaveChangesAsync();
        }

        public async Task<ProductReview?> GetByIdAsync(int id) =>
            await _db.ProductReviews
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<IEnumerable<ProductReview>> GetAllAsync() =>
            await _db.ProductReviews
                .Include(r => r.Product)
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

        public async Task<IEnumerable<ProductReview>> GetByProductIdAsync(int productId) =>
            await _db.ProductReviews
                .Include(r => r.Product)
                .AsNoTracking()
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

        public async Task<bool> UpdateAsync(ProductReview review)
        {
            var existing = await _db.ProductReviews.FindAsync(review.Id);
            if (existing is null) return false;

            existing.Rating = review.Rating;
            existing.Comment = review.Comment;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _db.ProductReviews.FindAsync(id);
            if (review is null) return false;

            _db.ProductReviews.Remove(review);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repositories
{
    public interface ISupplierRepository
    {
        Task AddAsync(Supplier supplier);
        Task<Supplier?> GetByIdAsync(int id);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<bool> UpdateAsync(Supplier supplier);
        Task<bool> DeleteAsync(int id);
    }
}

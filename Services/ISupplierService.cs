using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface ISupplierService
    {
        Task<(bool Success, string Message)> AddSupplierAsync(Supplier supplier);
        Task<(bool Success, string Message)> UpdateSupplierAsync(Supplier supplier);
        Task<(bool Success, string Message)> DeleteSupplierAsync(int id);
        Task<Supplier?> GetByIdAsync(int id);
        Task<IEnumerable<Supplier>> GetAllAsync();
    }
}

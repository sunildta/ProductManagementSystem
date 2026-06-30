using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repositories
{
    /// <summary>
    /// EF Core implementation of ISupplierRepository.
    /// Scoped lifetime — shares DbContext within a DI scope.
    /// </summary>
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _db;
        public SupplierRepository(ApplicationDbContext db) => _db = db;

        public async Task AddAsync(Supplier supplier)
        {
            await _db.Suppliers.AddAsync(supplier);
            await _db.SaveChangesAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id) =>
            await _db.Suppliers.FindAsync(id);

        public async Task<IEnumerable<Supplier>> GetAllAsync() =>
            await _db.Suppliers
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();

        public async Task<bool> UpdateAsync(Supplier supplier)
        {
            var existing = await _db.Suppliers.FindAsync(supplier.Id);
            if (existing is null) return false;

            existing.Name = supplier.Name;
            existing.Email = supplier.Email;
            existing.Phone = supplier.Phone;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _db.Suppliers.FindAsync(id);
            if (supplier is null) return false;

            _db.Suppliers.Remove(supplier);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

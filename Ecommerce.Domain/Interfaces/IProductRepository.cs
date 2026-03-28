using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    IQueryable<Product> GetProductsQueryable();
    Task<Product?> GetByIdAsync(int id);

    Task<Product> AddAsync(Product product);
    Task<bool> DeleteAsync(int? id);
}
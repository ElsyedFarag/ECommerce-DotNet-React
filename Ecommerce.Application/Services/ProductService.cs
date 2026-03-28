using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services;

public class ProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _repo.GetAllAsync();
    }
    public IQueryable<Product> GetProductsQueryable()
    {
        return _repo.GetProductsQueryable();
    }
    public async Task<Product?> GetProductById(int id)
    {
        return await _repo.GetByIdAsync(id);
    }
    public async Task<Product> AddProduct(Product product)
    {
        return await _repo.AddAsync(product);
    }

    public async Task<bool> DeleteAsync(int? id)
    {
        return  await _repo.DeleteAsync(id);
    }
}
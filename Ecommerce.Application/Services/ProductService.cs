using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;

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

    public async Task AddProduct(Product product)
    {
        await _repo.AddAsync(product);
    }
}
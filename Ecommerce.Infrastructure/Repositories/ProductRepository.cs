using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(c=>c.Category)
            .Include(im=>im.Images)
            .Include(vr=>vr.Variants)
            .Include(sp => sp.ProductSpecifications)
            .Include(rv=>rv.Reviews)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(c => c.Category)
            .Include(im => im.Images)
            .Include(vr => vr.Variants)
            .Include(sp => sp.ProductSpecifications)
            .Include(rv => rv.Reviews)
            .FirstOrDefaultAsync(x=>x.Id == id);
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }
}
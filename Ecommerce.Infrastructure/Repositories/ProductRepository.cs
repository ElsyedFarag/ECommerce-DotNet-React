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
            .AsNoTracking()
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

    public async Task<bool> DeleteAsync(int? id)
    {
        if(!id.HasValue) return false;
        var product = await _context.Products.FindAsync(id);
        if(product == null) return false;

        product.IsDeleted = true;
        product.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public IQueryable<Product> GetProductsQueryable()
    {
        return _context.Products
                     .Where(p => !p.IsDeleted)
                     .Include(p => p.Images)
                     .Include(p => p.Reviews)
                     .Include(p => p.Variants)
                     .Include(p => p.ProductSpecifications)
                     .Include(p => p.Category);
    }
}
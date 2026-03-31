using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var categories = await _context.Categories
            .Include(p=>p.Products)
            .ToListAsync();
        return categories;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .Include(p=>p.Products)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Category> AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<bool> IsValidAsync(Expression<Func<Category, bool>> expression)
    {
        return await _context.Categories.AnyAsync(expression);
    }

    public async Task<bool> DeleteAsync(int? id)
    {
        if (!id.HasValue) return false;
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
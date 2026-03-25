using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using System.Linq.Expressions;

namespace Ecommerce.Application.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<(IEnumerable<Category> Data, int TotalCount)> GetCategories(int pageNumber, int pageSize, string? search)
    {
        return await _repo.GetAllAsync(pageNumber, pageSize, search);
    }

    public async Task<Category?> GetCategoryById(int id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<bool> IsValidAsync(Expression<Func<Category,bool>> expression)
    {
        return await _repo.IsValidAsync(expression);
    }
    public async Task<Category> AddCategory(Category category)
    {
        var newCategory = await _repo.AddAsync(category);
        return newCategory;
    }
}
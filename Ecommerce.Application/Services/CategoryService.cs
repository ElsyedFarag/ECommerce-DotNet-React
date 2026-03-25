using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;

namespace Ecommerce.Application.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await _repo.GetAllAsync();
    }

    public async Task AddCategory(Category category)
    {
        await _repo.AddAsync(category);
    }
}
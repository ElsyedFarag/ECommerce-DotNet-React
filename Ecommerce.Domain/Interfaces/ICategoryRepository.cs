using Ecommerce.Domain.Entities;
using System.Linq.Expressions;

namespace Ecommerce.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<(IEnumerable<Category> Data, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, string? search);

    Task<Category?> GetByIdAsync(int id);
    Task<bool> IsValidAsync(Expression<Func<Category, bool>> expression);

    Task<Category> AddAsync(Category category);
}
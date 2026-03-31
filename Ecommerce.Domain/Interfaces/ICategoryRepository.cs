using Ecommerce.Domain.Entities;
using System.Linq.Expressions;

namespace Ecommerce.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int id);
    Task<bool> IsValidAsync(Expression<Func<Category, bool>> expression);

    Task<Category> AddAsync(Category category);

    Task<bool> DeleteAsync(int? id);
}
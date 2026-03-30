
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Interfaces;

public interface IOrdersRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order> AddAsync(Order order);
}

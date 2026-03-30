using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;

namespace Ecommerce.Application.Services;

public class OrderServices
{
    private readonly IOrdersRepository _repo;

    public OrderServices(IOrdersRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _repo.GetAllAsync();
    }
    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _repo.GetByIdAsync(id);
    }
    public async Task<Order> AddOrdersAsync(Order order)
    {
        return await _repo.AddAsync(order);
    }
}

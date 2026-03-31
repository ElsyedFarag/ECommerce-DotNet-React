using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByCustomerIdAsync(int customerId);
    Task<Cart> AddAsync(Cart cart);
    Task<Cart> UpdateAsync(Cart cart);
    Task DeleteAsync(int cartId);
}
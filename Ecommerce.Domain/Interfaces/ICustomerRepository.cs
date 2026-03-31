namespace Ecommerce.Domain.Interfaces;

using Ecommerce.Domain.Entities;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(string id);

    Task<Customer?> GetByUserIdAsync(string userId);

    Task<IEnumerable<Customer>> GetAllAsync();

    Task<bool> ExistsAsync(string id);

    Task AddAsync(Customer customer);

    Task UpdateAsync(Customer customer);

    Task DeleteAsync(string id);
}
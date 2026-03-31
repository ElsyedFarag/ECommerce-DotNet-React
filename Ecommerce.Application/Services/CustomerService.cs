using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;

namespace Ecommerce.Application.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(string id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<Customer?> GetCustomerByUserIdAsync(string userId)
    {
        return await _customerRepository.GetByUserIdAsync(userId);
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        await _customerRepository.AddAsync(customer);
        return customer;
    }

    public async Task<bool> UpdateCustomerAsync(Customer customer)
    {
        var exists = await _customerRepository.ExistsAsync(customer.Id);

        if (!exists)
            return false;

        await _customerRepository.UpdateAsync(customer);
        return true;
    }

    public async Task<bool> DeleteCustomerAsync(string id)
    {
        var exists = await _customerRepository.ExistsAsync(id);

        if (!exists)
            return false;

        await _customerRepository.DeleteAsync(id);
        return true;
    }
}
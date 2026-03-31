using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.Interfaces;
public interface IPaymentRepository
{
    Task<IEnumerable<OrderPayment>> GetPaymentsAsync();
    Task<OrderPayment> GetPaymentsByIdAsync(int id);
    Task<OrderPayment> GetPaymentByOrderIdAsync(int orderId);
    Task<OrderPayment> CreatePaymentAsync(OrderPayment payment);
    Task<string> CreatePaymentUrl(int orderId, decimal totalAmount);
}


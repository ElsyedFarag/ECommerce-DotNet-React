using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderPayment> CreatePaymentAsync(OrderPayment payment)
    {
        await _context.OrderPayments.AddAsync(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<string> CreatePaymentUrl(int orderId, decimal totalAmount)
    {
        var paymentUrl = $"https://payment-gateway.com/pay?order={orderId}&amount={totalAmount}";
        return paymentUrl;
    }

    public async Task<OrderPayment?> GetPaymentByOrderIdAsync(int orderId)
    {
        return await _context.OrderPayments.FirstOrDefaultAsync(x => x.OrderId == orderId);
    }

    public async Task<IEnumerable<OrderPayment>> GetPaymentsAsync()
    {
        return await _context.OrderPayments.ToListAsync();
    }

    public async Task<OrderPayment?> GetPaymentsByIdAsync(int id)
    {
        return await _context.OrderPayments.FirstOrDefaultAsync(x => x.Id == id);
    }

}

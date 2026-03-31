// PaymentService.cs
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Interfaces;

namespace Ecommerce.Application.Services;

public class PaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<string?> InitializePaymentAsync(Order order, PaymentMethod method)
    {
        // 1️⃣ إنشاء سجل الدفع
        var payment = new OrderPayment
        {
            OrderId = order.Id,
            Amount = order.TotalAmount,
            Method = method,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.CreatePaymentAsync(payment);

        // 2️⃣ الدفع COD لا يحتاج رابط
        if (method == PaymentMethod.CashOnDelivery)
            return null;

        // 3️⃣ أي طريقة دفع أونلاين
        var paymentUrl = await _paymentRepository.CreatePaymentUrl(order.Id, order.TotalAmount);
        return paymentUrl;
    }
}
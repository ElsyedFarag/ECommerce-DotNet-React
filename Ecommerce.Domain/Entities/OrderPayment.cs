using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.Entities;

public class OrderPayment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public PaymentMethod Method { get; set; }

    public PaymentStatus Status { get; set; }

    public decimal Amount { get; set; }

    public string? TransactionId { get; set; }

    public string? PaymentGateway { get; set; }

    public DateTime PaidAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    public Order Order { get; set; } = null!;
}

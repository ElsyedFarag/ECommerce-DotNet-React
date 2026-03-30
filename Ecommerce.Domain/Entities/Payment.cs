using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.Entities;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public PaymentMethod Method { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }

    public DateTime PaidAt { get; set; }
}
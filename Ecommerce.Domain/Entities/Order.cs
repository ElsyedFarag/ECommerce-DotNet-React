using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = Guid.NewGuid().ToString();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public decimal SubTotal { get; set; }

    public decimal Discount { get; set; }

    public decimal ShippingCost { get; set; }

    public decimal Tax { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Notes { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}

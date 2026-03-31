using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Tax { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public string Currency { get; set; } = "EGP";
    public string? CouponCode { get; set; }
    public string? AdminNotes { get; set; }
    public string? PaymentUrl { get; set; }

    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CashOnDelivery;
    public List<OrderItemDto> Items { get; set; } = new();

    public List<OrderPayment> Payments { get; set; } = new();

    public List<OrderShipment> Shipments { get; set; } = new();

    public List<OrderAddress> Addresses { get; set; } = new();

    public List<OrderStatusHistory> StatusHistory { get; set; } = new();
}

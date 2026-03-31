using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = Guid.NewGuid().ToString();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }



    // =====================
    // Customer
    // =====================

    public int? CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? CustomerEmail { get; set; }

    public string? CustomerPhone { get; set; }



    // =====================
    // Status
    // =====================

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;



    // =====================
    // Pricing
    // =====================

    public decimal SubTotal { get; set; }

    public decimal Discount { get; set; }

    public decimal ShippingCost { get; set; }

    public decimal Tax { get; set; }

    public decimal TotalAmount { get; set; }

    public string Currency { get; set; } = "EGP";



    // =====================
    // Coupon
    // =====================

    public string? CouponCode { get; set; }



    // =====================
    // Notes
    // =====================

    public string? Notes { get; set; }

    public string? AdminNotes { get; set; }



    // =====================
    // Navigation Properties
    // =====================
    public Customer Customer { get; set; } = null!;

    public List<OrderItem> Items { get; set; } = new();

    public List<OrderPayment> Payments { get; set; } = new();

    public List<OrderShipment> Shipments { get; set; } = new();

    public List<OrderAddress> Addresses { get; set; } = new();

    public List<OrderStatusHistory> StatusHistory { get; set; } = new();
}


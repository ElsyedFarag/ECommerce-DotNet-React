using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Dtos;

public class OrderCreateDto
{
    public int? CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? CustomerEmail { get; set; }

    public string? CustomerPhone { get; set; }

    public string? CouponCode { get; set; }
    public string Currency { get; set; } = "EGP";

    public string? Notes { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CashOnDelivery;

    public List<OrderItemCreateDto> Items { get; set; } = new();
    public List<OrderAddressCreateDto> Addresses { get; set; } = new();
}

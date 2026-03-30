using Ecommerce.Domain.Enums;


namespace Ecommerce.Application.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public string PaymentStatus { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Tax { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.Entities;

public class OrderStatusHistory
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public OrderStatus Status { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    public Order Order { get; set; } = null!;
}

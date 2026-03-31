namespace Ecommerce.Domain.Entities;

public class OrderShipment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string ShippingCompany { get; set; } = null!;

    public string TrackingNumber { get; set; } = null!;

    public decimal ShippingCost { get; set; }

    public DateTime? ShippedAt { get; set; }

    public DateTime? DeliveredAt { get; set; }



    public Order Order { get; set; } = null!;
}

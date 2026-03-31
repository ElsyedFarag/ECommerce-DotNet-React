namespace Ecommerce.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductSku { get; set; }

    public decimal Price { get; set; }

    public decimal? Discount { get; set; }

    public int Quantity { get; set; }

    public decimal Tax { get; set; }

    public decimal TotalPrice { get; set; }



    // Relations
    public Order Order { get; set; } = null!;
}
namespace Ecommerce.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal Tax { get; set; }

    public decimal Discount { get; set; }

    public decimal TotalPrice { get; set; }
}
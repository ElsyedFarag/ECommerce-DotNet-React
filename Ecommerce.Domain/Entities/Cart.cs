namespace Ecommerce.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = null!;
    public decimal SubTotal { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Customer Customer { get; set; } = null!;
    public List<CartItem> Items { get; set; } = new();
}

namespace Ecommerce.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public int CustomerId { get; set; } 
    public decimal SubTotal { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalAmount { get; set; }

    public List<CartItem> Items { get; set; } = new();
    public Customer Customer { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

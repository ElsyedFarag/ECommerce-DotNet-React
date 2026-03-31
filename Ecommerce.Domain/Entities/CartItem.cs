namespace Ecommerce.Domain.Entities;

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }           // الربط بالسلة
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string ProductSku { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }   // (Price - Discount + Tax) * Quantity
}
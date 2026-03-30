
namespace Ecommerce.Application.Dtos;

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Tax { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
}

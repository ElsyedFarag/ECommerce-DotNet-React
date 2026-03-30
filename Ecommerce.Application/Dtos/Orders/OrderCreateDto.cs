
namespace Ecommerce.Application.Dtos;

public class OrderCreateDto
{
    public string? Notes { get; set; }

    public List<OrderItemCreateDto> Items { get; set; } = new();
}

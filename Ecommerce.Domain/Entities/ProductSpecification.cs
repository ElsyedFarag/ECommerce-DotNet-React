namespace Ecommerce.Domain.Entities;
public class ProductSpecification
{
    public int Id { get; set; }

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public int ProductId { get; set; }

    public Product? Product { get; set; }
}
namespace Ecommerce.Domain.Entities;
public class ProductVariant
{
    public int Id { get; set; }

    public string Name { get; set; }   // Resistance

    public string Value { get; set; }  // 1K

    public decimal? Price { get; set; }

    public int Stock { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; }
}

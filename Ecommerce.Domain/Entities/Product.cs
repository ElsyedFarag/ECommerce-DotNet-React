namespace Ecommerce.Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public decimal? DiscountPrice { get; set; }

    public int Stock { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public ICollection<ProductVariant> Variants { get; set; }
    public ICollection<ProductSpecification> ProductSpecifications { get; set; }

    public ICollection<ProductReview> Reviews { get; set; }

    public ICollection<ProductImage> Images { get; set; }
}

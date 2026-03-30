namespace Ecommerce.Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!; // رابط SEO

    public string? Description { get; set; }

    public decimal Price { get; set; }
    public decimal TaxRate { get; set; }

    public decimal? DiscountPrice { get; set; }

    public string? SKU { get; set; }  // كود المنتج

    public bool ManageStock { get; set; } = true;

    public int Stock { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    /* ===== العلاقات ===== */

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

    public ICollection<ProductSpecification> ProductSpecifications { get; set; } = new List<ProductSpecification>();

    public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();

    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
}
using Ecommerce.Application.Dtos.Categories;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Dtos.Products
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? SKU { get; set; }  // كود المنتج
        public string Slug { get; set; } = null!; 
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
        public decimal? DiscountPrice { get; set; }
        public bool IsActive { get; set; } = true;
        public bool ManageStock { get; set; } = true;
        public int Stock { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public CategoryDto? Category { get; set; }

        public List<ProductVariantDto>? Variants { get; set; }
        public List<ProductImageDto>? Images { get; set; }
        public List<ProductReviewDto>? Reviews { get; set; }
        public List<ProductSpecificationDto>? Specifications { get; set; }
    }
}
using Ecommerce.Application.Dtos.Categories;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Dtos.Products
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Stock { get; set; }
        public CategoryDto Category { get; set; }

        public List<ProductVariantDto>? Variants { get; set; }
        public List<ProductImageDto>? Images { get; set; }
        public List<ProductReview>? Reviews { get; set; }
        public List<ProductSpecificationDto>? Specifications { get; set; }
    }
}
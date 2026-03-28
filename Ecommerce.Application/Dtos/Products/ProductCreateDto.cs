using Microsoft.AspNetCore.Http;

namespace Ecommerce.Application.Dtos.Products;
public class ProductCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }

    public List<ProductVariantDto>? Variants { get; set; }
    public List<IFormFile>? Images { get; set; }
    public int MainImageIndex { get; set; } = 0;

    public List<ProductSpecificationDto>? Specifications { get; set; }
}

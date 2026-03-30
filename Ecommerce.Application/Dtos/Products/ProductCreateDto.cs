using Microsoft.AspNetCore.Http;

namespace Ecommerce.Application.Dtos.Products;
public class ProductCreateDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public string? SKU { get; set; } 
    public string Slug { get; set; } = null!;
    public decimal TaxRate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool ManageStock { get; set; } = true;
    public List<ProductVariantDto>? Variants { get; set; }
    public List<IFormFile>? Images { get; set; }
    public int MainImageIndex { get; set; } = 0;

    public List<ProductSpecificationDto>? Specifications { get; set; }
}

using Ecommerce.Application.Dtos.Categories;
using Ecommerce.Application.Dtos.Products;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;
    public ProductsController(ProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _service.GetProducts();

        var responseDtos = products.Select(x => new ProductDetailsDto
        {
            Id = x.Id,
            Name = x.Name,
            Slug = x.Slug,
            SKU = x.SKU,
            IsActive = x.IsActive,
            IsDeleted = x.IsDeleted,
            DeletedAt = x.DeletedAt,
            ManageStock = x.ManageStock,
            Description = x.Description ?? "-",
            Price = x.Price,
            DiscountPrice = x.DiscountPrice,
            TaxRate = x.TaxRate,
            Stock = x.Stock,
            Reviews = x.Reviews.Select(r => new ProductReviewDto
            {
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UserName = r.UserName
            }).ToList(),
            Specifications = x.ProductSpecifications.Select(s => new ProductSpecificationDto
            {
                Key = s.Key,
                Value = s.Value
            }).ToList(),
            Images = x.Images.Select(i => new ProductImageDto
            {
                Url = i.Url,
                IsMain = i.IsMain
            }).ToList(),
            Category = new CategoryDto
            {
                Name = x.Category?.Name ?? "-",
                Description = x.Category!.Description ?? "-"
            },
            Variants = x.Variants.Select(v => new ProductVariantDto
            {
                Value = v.Value,
                Name = v.Name,
                Price = v.Price,
            }).ToList()
        });

        return Ok( responseDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _service.GetProductById(id);

        if (product == null)
            return NotFound("Product not found");
        var responseDto = new ProductDetailsDto
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            SKU = product.SKU,
            IsActive = product.IsActive,
            IsDeleted = product.IsDeleted,
            DeletedAt = product.DeletedAt,
            ManageStock = product.ManageStock,
            Description = product.Description ?? "-",
            Price = product.Price,
            DiscountPrice = product.DiscountPrice,
            TaxRate = product.TaxRate,
            Stock = product.Stock,
            Reviews = product.Reviews.Select(r => new ProductReviewDto
            {
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UserName = r.UserName
            }).ToList(),
            Specifications = product.ProductSpecifications.Select(s => new ProductSpecificationDto
            {
                Key = s.Key,
                Value = s.Value
            }).ToList(),
            Images = product.Images.Select(i => new ProductImageDto
            {
                Url = i.Url,
                IsMain = i.IsMain
            }).ToList(),
            Category = new CategoryDto
            {
                Name = product.Category?.Name ?? "-",
                Description = product.Category?.Description ?? "-"
            },
            Variants = product.Variants.Select(v => new ProductVariantDto
            {
                Value = v.Value,
                Name = v.Name,
                Price = v.Price,
            }).ToList()
        };

        return Ok(responseDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newProduct = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            DiscountPrice = model.DiscountPrice,
            Stock = model.Stock,
            ManageStock = model.ManageStock,
            SKU = model.SKU,
            Slug = model.Slug,
            TaxRate = model.TaxRate,
            IsActive = model.IsActive,
            CreatedAt = DateTime.UtcNow, 
            CategoryId = model.CategoryId,

            Variants = model.Variants?.Select(v => new ProductVariant
            {
                Name = v.Name,
                Value = v.Value,
                Price = v.Price
            }).ToList() ?? new List<ProductVariant>(),

            ProductSpecifications = model.Specifications?.Select(s => new ProductSpecification
            {
                Key = s.Key,
                Value = s.Value
            }).ToList() ?? new List<ProductSpecification>()
        };

        /* ================== رفع الصور ================== */

        if (model.Images != null && model.Images.Any())
        {
            newProduct.Images = new List<ProductImage>();

            for (int i = 0; i < model.Images.Count; i++)
            {
                var img = model.Images[i];

                if (img != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");

                    // التأكد من وجود المجلد
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var path = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    newProduct.Images.Add(new ProductImage
                    {
                        Url = "/images/products/" + fileName,
                        IsMain = i == model.MainImageIndex
                    });
                }
            }
        }

        /* ================== حفظ المنتج ================== */

        var result = await _service.AddProduct(newProduct);

        if (result == null)
            return BadRequest("Failed to create product");

        /* ================== تحويل للـ DTO ================== */

        var responseDto = new ProductDetailsDto
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description ?? "-",
            Price = result.Price,
            DiscountPrice = result.DiscountPrice,
            Stock = result.Stock,
            Reviews = result.Reviews.Select(r => new ProductReviewDto
            {
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UserName = r.UserName
            }).ToList(),
            Specifications = result.ProductSpecifications.Select(s => new ProductSpecificationDto
            {
                Key = s.Key,
                Value = s.Value
            }).ToList(),
            Images = result.Images.Select(i => new ProductImageDto
            {
                Url = i.Url,
                IsMain = i.IsMain
            }).ToList(),
            Category = new CategoryDto
            {
                Name = result.Category?.Name ?? "-",
                Description = result.Category?.Description ?? "-"
            },
            Variants = result.Variants.Select(v => new ProductVariantDto
            {
                Value = v.Value,
                Name = v.Name,
                Price = v.Price,
            }).ToList()
        };

        return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, responseDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid product ID.");

        var isDeleted = await _service.DeleteAsync(id);

        if (!isDeleted)
            return NotFound("Product not found or already deleted.");

        return Ok("Product deleted successfully!");
    }

}
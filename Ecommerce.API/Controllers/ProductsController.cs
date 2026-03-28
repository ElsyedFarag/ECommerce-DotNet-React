using AutoMapper;
using Ecommerce.Application.Dtos.Categories;
using Ecommerce.Application.Dtos.Products;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> Get(
    [FromQuery] string search = "",
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var query = _service.GetProductsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
        }

        var totalItems = await query.CountAsync();

        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var responseDtos = products.Select(x => new ProductDetailsDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            DiscountPrice = x.DiscountPrice,
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
                Name = x.Category.Name,
                Description = x.Category.Description
            },
            Variants = x.Variants.Select(v => new ProductVariantDto
            {
                Value = v.Value,
                Name = v.Name,
                Price = v.Price,
            }).ToList()
        });

        return Ok(new
        {
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
            Items = responseDtos
        });
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetProductById(id);

        if (product == null)
            return NotFound("Product not found");
        var responseDto = new ProductDetailsDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            DiscountPrice = product.DiscountPrice,
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
                Name = product.Category.Name,
                Description = product.Category.Description
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
    [HttpPost("AddProduct")]
    public async Task<IActionResult> Create([FromForm] ProductCreateDto model)
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
            Description = result.Description,
            Price = result.Price,
            DiscountPrice = result.DiscountPrice,
            Stock = result.Stock,

            Reviews = result.Reviews?.Select(r => new ProductReviewDto
            {
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UserName = r.UserName
            }).ToList() ?? new List<ProductReviewDto>(),

            Specifications = result.ProductSpecifications?.Select(s => new ProductSpecificationDto
            {
                Key = s.Key,
                Value = s.Value
            }).ToList() ?? new List<ProductSpecificationDto>(),

            Images = result.Images?.Select(i => new ProductImageDto
            {
                Url = i.Url,
                IsMain = i.IsMain
            }).ToList() ?? new List<ProductImageDto>(),

            Category = result.Category == null ? null : new CategoryDto
            {
                Name = result.Category.Name,
                Description = result.Category.Description
            },

            Variants = result.Variants?.Select(v => new ProductVariantDto
            {
                Name = v.Name,
                Value = v.Value,
                Price = v.Price
            }).ToList() ?? new List<ProductVariantDto>()
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, responseDto);
    }

    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid product ID.");

        var isDeleted = await _service.DeleteAsync(id);

        if (!isDeleted)
            return NotFound("Product not found or already deleted.");

        return Ok("Product deleted successfully!");
    }

}
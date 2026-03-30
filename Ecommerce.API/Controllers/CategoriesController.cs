using Ecommerce.Application.Dtos.Categories;
using Ecommerce.Application.Dtos.Products;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _service;

    public CategoriesController(CategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categories = await _service.GetCategories();

        var result = categories.Select(c => new CategoryDetailsDto
        {
            Name = c.Name,
            Description = c.Description,
            Products = c.Products.Select(product => new ProductDetailsDto
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
            }).ToList()

        }).ToList();

        return Ok(result);
    }

    [HttpGet("GetCategoryById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _service.GetCategoryById(id);

        if (category == null)
            return NotFound("Category not found");

        var result = new CategoryDetailsDto
        {
            Name = category.Name,
            Description = category.Description,
            Products = category.Products.Select(product => new ProductDetailsDto
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
            }).ToList()

        };
        return Ok(result);
    }

    [HttpPost("AddCategory")]
    public async Task<IActionResult> Create(CategoryDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exists = await _service.IsValidAsync(c => c.Name == model.Name);

        if (exists)
            return BadRequest("Category with the same name already exists");

        var category = new Category
        {
            Name = model.Name,
            Description = model.Description
        };

        var result = await _service.AddCategory(category);


        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            result);
    }
}
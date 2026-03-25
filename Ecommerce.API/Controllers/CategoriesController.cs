using Ecommerce.Application.Dtos.Categories;
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
    public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10, string? search = null)
    {
        var result = await _service.GetCategories(pageNumber, pageSize, search);

        return Ok(new
        {
            totalCount = result.TotalCount,
            pageNumber,
            pageSize,
            data = result.Data
        });
    }

    [HttpGet("GetCategoryById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _service.GetCategoryById(id);

        if (category == null)
            return NotFound("Category not found");


        return Ok(category);
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
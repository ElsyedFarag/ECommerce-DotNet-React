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
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        await _service.AddCategory(category);
        return Ok();
    }
}
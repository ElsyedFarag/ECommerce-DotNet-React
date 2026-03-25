using AutoMapper;
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
    private readonly IMapper _mapper;
    public ProductsController(ProductService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var products = await _service.GetProducts();
        var responseDtos = _mapper.Map<IEnumerable<ProductDetailsDto>>(products);
        return Ok(responseDtos);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetProductById(id);

        if (product == null)
            return NotFound("Product not found");

        var responseDto = _mapper.Map<ProductDetailsDto>(product);

        return Ok(responseDto);
    }
    [HttpPost("AddProduct")]
    public async Task<IActionResult> Create(ProductCreateDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = _mapper.Map<Product>(model);

        var result = await _service.AddProduct(product);

        var responseDto = _mapper.Map<ProductDetailsDto>(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, responseDto);
    }
}
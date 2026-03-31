using Ecommerce.Application.Dtos.Carts;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<CartDto>> GetCart(string customerId)
    {
        var cart = await _cartService.GetOrCreateCartAsync(customerId);

        var customer = cart.Customer?.User;
        var customerName = $"{customer?.FirstName} {customer?.LastName}".Trim();

        var cartDto = new CartDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            CustomerName = string.IsNullOrWhiteSpace(customerName) ? "Unknown" : customerName,
            CustomerEmail = customer?.Email ?? "Unknown",
            CustomerPhone = customer?.PhoneNumber ?? "Unknown",
            SubTotal = cart.SubTotal,
            TotalDiscount = cart.TotalDiscount,
            TotalTax = cart.TotalTax,
            TotalAmount = cart.TotalAmount,
            Items = cart.Items?.Select(i => new CartItemDto
            {
                Id = i.Id, 
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ProductSku = i.Product.SKU ?? "Unknown",
                Quantity = i.Quantity,
                Price = i.Price,
                Discount = i.Discount,
                Tax = i.Tax,
                TotalPrice = i.TotalPrice
            }).ToList() ?? new List<CartItemDto>()
        };

        return Ok(cartDto);
    }

    [HttpPost("{customerId}/add")]
    public async Task<IActionResult> AddItem(string customerId, [FromBody] CartItemCreateDto itemDto)
    {
        var cart = await _cartService.AddItemAsync(customerId, itemDto.ProductId, itemDto.Quantity);

        var customer = cart.Customer?.User;
        var customerName = $"{customer?.FirstName} {customer?.LastName}".Trim();

        var cartDto = new CartDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            CustomerName = string.IsNullOrWhiteSpace(customerName) ? "Unknown" : customerName,
            CustomerEmail = customer?.Email ?? "Unknown",
            CustomerPhone = customer?.PhoneNumber ?? "Unknown",
            SubTotal = cart.SubTotal,
            TotalDiscount = cart.TotalDiscount,
            TotalTax = cart.TotalTax,
            TotalAmount = cart.TotalAmount,
            Items = cart.Items?.Select(i => new CartItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ProductSku = i.Product.SKU ?? "Unknown",
                Quantity = i.Quantity,
                Price = i.Price,
                Discount = i.Discount,
                Tax = i.Tax,
                TotalPrice = i.TotalPrice
            }).ToList() ?? new List<CartItemDto>()
        };
        return Ok(cartDto);
    }
    [HttpPost("{customerId}/update")]
    public async Task<IActionResult> UpdateItemQuantity(string customerId, [FromBody] CartItemUpdateDto itemDto)
    {
        var cart = await _cartService.UpdateItemQuantityAsync(customerId, itemDto.ProductId, itemDto.Quantity);
        var customer = cart.Customer?.User;
        var customerName = $"{customer?.FirstName} {customer?.LastName}".Trim();

        var cartDto = new CartDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            CustomerName = string.IsNullOrWhiteSpace(customerName) ? "Unknown" : customerName,
            CustomerEmail = customer?.Email ?? "Unknown",
            CustomerPhone = customer?.PhoneNumber ?? "Unknown",
            SubTotal = cart.SubTotal,
            TotalDiscount = cart.TotalDiscount,
            TotalTax = cart.TotalTax,
            TotalAmount = cart.TotalAmount,
            Items = cart.Items?.Select(i => new CartItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ProductSku = i.Product.SKU ?? "Unknown",
                Quantity = i.Quantity,
                Price = i.Price,
                Discount = i.Discount,
                Tax = i.Tax,
                TotalPrice = i.TotalPrice
            }).ToList() ?? new List<CartItemDto>()
        };
        return Ok(cartDto);
    }

    [HttpPost("{customerId}/remove")]
    public async Task<IActionResult> RemoveItem(string customerId, [FromBody] CartItemDto itemDto)
    {
        var cart = await _cartService.RemoveItemAsync(customerId, itemDto.ProductId);
        var customer = cart.Customer?.User;
        var customerName = $"{customer?.FirstName} {customer?.LastName}".Trim();

        var cartDto = new CartDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            CustomerName = string.IsNullOrWhiteSpace(customerName) ? "Unknown" : customerName,
            CustomerEmail = customer?.Email ?? "Unknown",
            CustomerPhone = customer?.PhoneNumber ?? "Unknown",
            SubTotal = cart.SubTotal,
            TotalDiscount = cart.TotalDiscount,
            TotalTax = cart.TotalTax,
            TotalAmount = cart.TotalAmount,
            Items = cart.Items?.Select(i => new CartItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ProductSku = i.Product.SKU ?? "Unknown",
                Quantity = i.Quantity,
                Price = i.Price,
                Discount = i.Discount,
                Tax = i.Tax,
                TotalPrice = i.TotalPrice
            }).ToList() ?? new List<CartItemDto>()
        };
        return Ok(cartDto);
    }

    [HttpPost("{customerId}/clear")]
    public async Task<IActionResult> ClearCart(string customerId)
    {
        var cart = await _cartService.ClearCartAsync(customerId);
        var customer = cart.Customer?.User;
        var customerName = $"{customer?.FirstName} {customer?.LastName}".Trim();

        var cartDto = new CartDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            CustomerName = string.IsNullOrWhiteSpace(customerName) ? "Unknown" : customerName,
            CustomerEmail = customer?.Email ?? "Unknown",
            CustomerPhone = customer?.PhoneNumber ?? "Unknown",
            SubTotal = cart.SubTotal,
            TotalDiscount = cart.TotalDiscount,
            TotalTax = cart.TotalTax,
            TotalAmount = cart.TotalAmount,
            Items = cart.Items?.Select(i => new CartItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ProductSku = i.Product.SKU ?? "Unknown",
                Quantity = i.Quantity,
                Price = i.Price,
                Discount = i.Discount,
                Tax = i.Tax,
                TotalPrice = i.TotalPrice
            }).ToList() ?? new List<CartItemDto>()
        };
        return Ok(cartDto);
    }
}
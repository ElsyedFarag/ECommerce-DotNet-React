using Ecommerce.Application.Dtos;
using Ecommerce.Application.Dtos.Carts;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;
    private readonly OrderServices _orderServices;

    public CartController(CartService cartService, OrderServices orderServices)
    {
        _cartService = cartService;
        _orderServices = orderServices;
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCart(int customerId)
    {
        var cart = await _cartService.GetOrCreateCartAsync(customerId);
        return Ok(cart);
    }

    [HttpPost("{customerId}/add")]
    public async Task<IActionResult> AddItem(int customerId, [FromBody] CartItemDto itemDto)
    {
        var cart = await _cartService.AddItemAsync(customerId, itemDto.ProductId, itemDto.Quantity);
        return Ok(cart);
    }

    [HttpPost("{customerId}/remove")]
    public async Task<IActionResult> RemoveItem(int customerId, [FromBody] CartItemDto itemDto)
    {
        var cart = await _cartService.RemoveItemAsync(customerId, itemDto.ProductId);
        return Ok(cart);
    }

    [HttpPost("{customerId}/clear")]
    public async Task<IActionResult> ClearCart(int customerId)
    {
        var cart = await _cartService.ClearCartAsync(customerId);
        return Ok(cart);
    }

    [HttpPost("{customerId}/checkout")]
    public async Task<IActionResult> Checkout(int customerId, [FromBody] OrderCreateDto orderDto)
    {
        // تحويل Cart → Order
        var cart = await _cartService.GetOrCreateCartAsync(customerId);

        if (!cart.Items.Any())
            return BadRequest("Cart is empty");

        // ملء الـ orderDto من Cart
        orderDto.Items = cart.Items.Select(i => new OrderItemCreateDto
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity
        }).ToList();

        var order = await _orderServices.CreateOrderAsync(orderDto);

        // مسح السلة بعد إنشاء الطلب
        await _cartService.ClearCartAsync(customerId);

        return Ok(order);
    }
}

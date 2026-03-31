using Ecommerce.Application.Dtos;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly OrderServices _orderServices;
    private readonly ProductService _productService;

    public OrdersController(ProductService productService, OrderServices orderServices)
    {
        _productService = productService;
        _orderServices = orderServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var result = await _orderServices.GetOrdersAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderServices.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound();

        var result = new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CreatedAt = order.CreatedAt,
            Status = order.Status.ToString(),
            PaymentStatus = order.PaymentStatus.ToString(),
            SubTotal = order.SubTotal,
            Discount = order.Discount,
            ShippingCost = order.ShippingCost,
            Tax = order.Tax,
            TotalAmount = order.TotalAmount,
            Notes = order.Notes,
            AdminNotes = order.AdminNotes,
            CouponCode = order.CouponCode,
            Currency = order.Currency,
            CustomerId = order.CustomerId,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            CustomerPhone = order.CustomerPhone,
            UpdatedAt = order.UpdatedAt,
            Items = order.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ProductSku = i.ProductSku,
                Quantity = i.Quantity,
                Price = i.Price,
                Tax = i.Tax,
                Discount = i.Discount.Value,
                TotalPrice = i.TotalPrice,
            }).ToList()
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderCreateDto orderDto)
    {
        if (orderDto.Items == null || !orderDto.Items.Any())
            return BadRequest("Order must contain items");

        try
        {
            var result = await _orderServices.CreateOrderAsync(orderDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // يمكن تحسينه لاحقاً بإرجاع نوع الخطأ المناسب
            return BadRequest(new { error = ex.Message });
        }
        
    }
}

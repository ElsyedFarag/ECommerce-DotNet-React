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
    public async Task<IActionResult> Get()
    {
        var result = await _orderServices.GetOrdersAsync();
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
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
            Items = order.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price,
                Tax = i.Tax,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderCreateDto orderDto)
    {
        if (orderDto.Items == null || !orderDto.Items.Any())
            return BadRequest("Order must contain items");

        var order = new Order
        {
            CreatedAt = DateTime.UtcNow,
            Notes = orderDto.Notes,
            Status = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            Items = new List<OrderItem>()
        };

        decimal subTotal = 0;
        decimal totalTax = 0;
        decimal totalDiscount = 0;

        foreach (var item in orderDto.Items)
        {
            if (item.Quantity <= 0)
                return BadRequest($"Invalid quantity for product {item.ProductId}");

            var product = await _productService.GetProductById(item.ProductId);
            if (product == null)
                return BadRequest($"Product {item.ProductId} not found");

            if (!product.IsActive || product.IsDeleted)
                return BadRequest($"Product {product.Name} is not available");

            if (product.ManageStock && item.Quantity > product.Stock)
                return BadRequest($"Not enough stock for product {product.Name}");

            // السعر الفعلي بعد الخصم
            decimal effectivePrice = product.DiscountPrice.HasValue && product.DiscountPrice.Value > 0 && product.DiscountPrice.Value < product.Price
                                    ? product.DiscountPrice.Value
                                    : product.Price;

            decimal itemSubTotal = effectivePrice * item.Quantity;
            decimal tax = itemSubTotal * product.TaxRate / 100;
            decimal discount = (product.Price - effectivePrice) * item.Quantity;
            decimal total = itemSubTotal + tax;

            subTotal += itemSubTotal;
            totalTax += tax;
            totalDiscount += discount;

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                Price = product.Price,
                Discount = discount,
                Tax = tax,
                TotalPrice = total
            });

            // تحديث المخزون
            if (product.ManageStock)
                product.Stock -= item.Quantity;
        }

        order.SubTotal = subTotal;
        order.Tax = totalTax;
        order.Discount = totalDiscount;
        order.TotalAmount = subTotal + totalTax; // خصم محسوب لكل عنصر، لا تحتاج خصم آخر

        // حفظ الطلب
        var result = await _orderServices.AddOrdersAsync(order);

        // تحويل للـ DTO
        var resultDto = new OrderDto
        {
            Id = result.Id,
            OrderNumber = result.OrderNumber,
            CreatedAt = result.CreatedAt,
            Status = result.Status.ToString(),
            PaymentStatus = result.PaymentStatus.ToString(),
            SubTotal = result.SubTotal,
            Discount = result.Discount,
            ShippingCost = result.ShippingCost,
            Tax = result.Tax,
            TotalAmount = result.TotalAmount,
            Notes = result.Notes,
            Items = result.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price,
                Discount = i.Discount,
                Tax = i.Tax,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        return CreatedAtAction(nameof(Get), new { id = result.Id }, resultDto);
    }
}

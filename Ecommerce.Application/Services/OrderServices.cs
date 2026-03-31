// OrderServices.cs
using Ecommerce.Application.Dtos;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Interfaces;

namespace Ecommerce.Application.Services;

public class OrderServices
{
    private readonly IOrdersRepository _repo;
    private readonly ProductService _productService;
    private readonly PaymentService _paymentService;

    public OrderServices(IOrdersRepository repo,
                         ProductService productService,
                         PaymentService paymentService)
    {
        _repo = repo;
        _productService = productService;
        _paymentService = paymentService;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
        => await _repo.GetAllAsync();

    public async Task<Order?> GetOrderByIdAsync(int id)
        => await _repo.GetByIdAsync(id);

    public async Task<OrderDto> CreateOrderAsync(OrderCreateDto orderDto)
    {
        // 1️⃣ التحقق من صحة البيانات
        if (orderDto.Items == null || !orderDto.Items.Any())
            throw new Exception("Order must contain items");

        if (orderDto.Addresses == null || !orderDto.Addresses.Any())
            throw new Exception("Order must contain at least one address");

        // 2️⃣ إنشاء الطلب الأساسي
        var order = new Order
        {
            Currency = orderDto.Currency ?? "EGP",
            CustomerId = orderDto.CustomerId,
            CustomerName = orderDto.CustomerName,
            CustomerEmail = orderDto.CustomerEmail,
            CustomerPhone = orderDto.CustomerPhone,
            CouponCode = orderDto.CouponCode,
            CreatedAt = DateTime.UtcNow,
            Notes = orderDto.Notes,
            Status = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            Items = new List<OrderItem>(),
            Addresses = orderDto.Addresses.Select(a => new OrderAddress
            {
                FullName = a.FullName,
                Country = a.Country,
                City = a.City,
                Phone = a.Phone,
                AddressLine = a.AddressLine,
                IsBillingAddress = a.IsBillingAddress,
                IsShippingAddress = a.IsShippingAddress,
                PostalCode = a.PostalCode
            }).ToList(),
            StatusHistory = new List<OrderStatusHistory>
            {
                new OrderStatusHistory
                {
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                }
            }
        };

        // 3️⃣ حساب الأسعار لكل عنصر
        decimal subTotal = 0, totalTax = 0, totalDiscount = 0;

        foreach (var item in orderDto.Items)
        {
            var product = await _productService.GetProductById(item.ProductId)
                          ?? throw new Exception($"Product {item.ProductId} not found");

            if (!product.IsActive || product.IsDeleted)
                throw new Exception($"Product {product.Name} is not available");

            if (product.ManageStock && item.Quantity > product.Stock)
                throw new Exception($"Not enough stock for product {product.Name}");

            var effectivePrice = product.DiscountPrice.HasValue &&
                                 product.DiscountPrice.Value > 0 &&
                                 product.DiscountPrice.Value < product.Price
                                 ? product.DiscountPrice.Value
                                 : product.Price;

            decimal itemSubTotal = effectivePrice * item.Quantity;
            decimal tax = itemSubTotal * product.TaxRate / 100;
            decimal discount = (product.Price - effectivePrice) * item.Quantity;

            subTotal += itemSubTotal;
            totalTax += tax;
            totalDiscount += discount;

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSku = product.SKU,
                Quantity = item.Quantity,
                Price = product.Price,
                Discount = discount,
                Tax = tax,
                TotalPrice = itemSubTotal + tax
            });
        }

        order.SubTotal = subTotal;
        order.Tax = totalTax;
        order.Discount = totalDiscount;
        order.TotalAmount = subTotal + totalTax - totalDiscount;

        // 4️⃣ حفظ الطلب
        await _repo.AddAsync(order);

        // 5️⃣ إنشاء الدفع
        var paymentUrl = await _paymentService.InitializePaymentAsync(order, orderDto.PaymentMethod);

        // 6️⃣ إذا الدفع COD، تأكيد الطلب مباشرة
        if (orderDto.PaymentMethod == PaymentMethod.CashOnDelivery)
        {
            order.Status = OrderStatus.Confirmed;
            await UpdateOrderAsync(order);
        }

        // 7️⃣ إعادة DTO
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            TotalAmount = order.TotalAmount,
            PaymentMethod = orderDto.PaymentMethod,
            Status = order.Status.ToString(),
            PaymentStatus = order.PaymentStatus.ToString(),
            PaymentUrl = paymentUrl
        };
    }

    public async Task<Order?> UpdateOrderAsync(Order order)
        => await _repo.UpdateAsync(order);
}
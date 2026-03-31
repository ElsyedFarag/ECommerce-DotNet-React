using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;

namespace Ecommerce.Application.Services;

public class CartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ProductService _productService;
    private readonly CustomerService _customerService;
    public CartService(ICartRepository cartRepository,
        ProductService productService,
        CustomerService customerService)
    {
        _cartRepository = cartRepository;
        _productService = productService;
        _customerService = customerService;
    }

    public async Task<Cart> GetOrCreateCartAsync(string customerId)
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerId);

        if (customer is null)
            throw new KeyNotFoundException($"Customer with id '{customerId}' not found");

        var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);

        if (cart is not null)
            return cart;

        cart = new Cart
        {
            CustomerId = customerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _cartRepository.AddAsync(cart);

        return cart;
    }

    public async Task<Cart> AddItemAsync(string customerId, int productId, int quantity)
    {

        if (quantity <= 0)
            throw new Exception("Quantity must be greater than zero");

        var customer = await _customerService.GetCustomerByIdAsync(customerId);

        if (customer is null)
            throw new KeyNotFoundException($"Customer with id '{customerId}' not found");

        var cart = await GetOrCreateCartAsync(customerId);
        var product = await _productService.GetProductById(productId)
                      ?? throw new Exception("Product not found");

        if (!product.IsActive || product.IsDeleted)
            throw new Exception("Product not available");

        var effectivePrice = product.DiscountPrice.HasValue && product.DiscountPrice.Value > 0 && product.DiscountPrice.Value < product.Price
            ? product.DiscountPrice.Value
            : product.Price;

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            existingItem.Price = product.Price;
            existingItem.Discount = product.Price - effectivePrice;
            existingItem.Tax = effectivePrice * product.TaxRate / 100;
            existingItem.TotalPrice = (effectivePrice + existingItem.Tax) * existingItem.Quantity;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                Quantity = quantity,
                Price = product.Price,
                Discount = product.Price - effectivePrice,
                Tax = effectivePrice * product.TaxRate / 100,
                TotalPrice = (effectivePrice + (effectivePrice * product.TaxRate / 100)) * quantity
            });
        }

        // إعادة حساب totals
        cart.SubTotal = cart.Items.Sum(i => i.Price * i.Quantity);
        cart.TotalDiscount = cart.Items.Sum(i => i.Discount * i.Quantity);
        cart.TotalTax = cart.Items.Sum(i => i.Tax * i.Quantity);
        cart.TotalAmount = cart.SubTotal + cart.TotalTax - cart.TotalDiscount;
        cart.UpdatedAt = DateTime.UtcNow;

        await _cartRepository.UpdateAsync(cart);
        return cart;
    }
    public async Task<Cart> UpdateItemQuantityAsync(string customerId, int productId, int quantity)
    {
        if (quantity < 0)
            throw new Exception("Quantity cannot be negative");

        var cart = await GetOrCreateCartAsync(customerId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item == null)
            throw new KeyNotFoundException("Item not found in cart");

        if (quantity == 0)
        {
            // إزالة العنصر إذا كانت الكمية صفر
            cart.Items.Remove(item);
        }
        else
        {
            var product = await _productService.GetProductById(productId)
                          ?? throw new Exception("Product not found");

            var effectivePrice = product.DiscountPrice.HasValue && product.DiscountPrice.Value > 0 && product.DiscountPrice.Value < product.Price
                ? product.DiscountPrice.Value
                : product.Price;

            item.Quantity = quantity;
            item.Price = product.Price;
            item.Discount = product.Price - effectivePrice;
            item.Tax = effectivePrice * product.TaxRate / 100;
            item.TotalPrice = (effectivePrice + item.Tax) * item.Quantity;
        }

        // إعادة حساب totals
        cart.SubTotal = cart.Items.Sum(i => i.Price * i.Quantity);
        cart.TotalDiscount = cart.Items.Sum(i => i.Discount * i.Quantity);
        cart.TotalTax = cart.Items.Sum(i => i.Tax * i.Quantity);
        cart.TotalAmount = cart.SubTotal + cart.TotalTax - cart.TotalDiscount;
        cart.UpdatedAt = DateTime.UtcNow;

        await _cartRepository.UpdateAsync(cart);
        return cart;
    }
    public async Task<Cart> RemoveItemAsync(string customerId, int productId)
    {
        var cart = await GetOrCreateCartAsync(customerId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            cart.Items.Remove(item);
            cart.SubTotal = cart.Items.Sum(i => i.Price * i.Quantity);
            cart.TotalDiscount = cart.Items.Sum(i => i.Discount * i.Quantity);
            cart.TotalTax = cart.Items.Sum(i => i.Tax * i.Quantity);
            cart.TotalAmount = cart.SubTotal + cart.TotalTax - cart.TotalDiscount;
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateAsync(cart);
        }
        return cart;
    }

    public async Task<Cart> ClearCartAsync(string customerId)
    {
        var cart = await GetOrCreateCartAsync(customerId);
        cart.Items.Clear();
        cart.SubTotal = 0;
        cart.TotalDiscount = 0;
        cart.TotalTax = 0;
        cart.TotalAmount = 0;
        cart.UpdatedAt = DateTime.UtcNow;
        await _cartRepository.UpdateAsync(cart);
        return cart;
    }
}
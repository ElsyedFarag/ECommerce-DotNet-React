using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;

namespace Ecommerce.Application.Services;

public class CartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ProductService _productService;

    public CartService(ICartRepository cartRepository, ProductService productService)
    {
        _cartRepository = cartRepository;
        _productService = productService;
    }

    public async Task<Cart> GetOrCreateCartAsync(int customerId)
    {
        var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
        if (cart == null)
        {
            cart = new Cart { CustomerId = customerId };
            await _cartRepository.AddAsync(cart);
        }
        return cart;
    }

    public async Task<Cart> AddItemAsync(int customerId, int productId, int quantity)
    {
        if (quantity <= 0)
            throw new Exception("Quantity must be greater than zero");

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
                ProductName = product.Name,
                ProductSku = product.SKU,
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

    public async Task<Cart> RemoveItemAsync(int customerId, int productId)
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

    public async Task<Cart> ClearCartAsync(int customerId)
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
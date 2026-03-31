using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ProductSpecification> ProductSpecifications { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<Category> Categories { get; set; }


    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderPayment> OrderPayments { get; set; }
    public DbSet<OrderAddress> OrderAddresses { get; set; }
    public DbSet<OrderShipment> OrderShipments { get; set; }
    public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }


}
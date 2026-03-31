namespace Ecommerce.Domain.Entities;

public class Customer
{
   public string Id { get; set; } = Guid.NewGuid().ToString(); // أو نفس نوع IdentityUser.Id
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

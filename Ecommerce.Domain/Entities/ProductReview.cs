namespace Ecommerce.Domain.Entities;
public class ProductReview
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }

    public string UserName { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; }
}

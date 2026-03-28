

namespace Ecommerce.Application.Dtos.Products;

public class ProductReviewDto
{

    public int Rating { get; set; }

    public string Comment { get; set; }

    public string UserName { get; set; }

    public DateTime CreatedAt { get; set; }

}

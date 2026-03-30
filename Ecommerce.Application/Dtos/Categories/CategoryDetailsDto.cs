using Ecommerce.Application.Dtos.Products;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.Dtos.Categories;

public class CategoryDetailsDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    public ICollection<ProductDetailsDto>? Products { get; set; }

}
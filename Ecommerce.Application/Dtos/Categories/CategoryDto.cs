
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.Dtos.Categories;

public class CategoryDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }
}


namespace Ecommerce.Application.Dtos;

public class OrderAddressCreateDto
{
    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string AddressLine { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string? PostalCode { get; set; }

    public bool IsShippingAddress { get; set; }

    public bool IsBillingAddress { get; set; }
}
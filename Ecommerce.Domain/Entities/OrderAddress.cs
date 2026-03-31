namespace Ecommerce.Domain.Entities;

public class OrderAddress
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string AddressLine { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string? PostalCode { get; set; }

    public bool IsShippingAddress { get; set; }

    public bool IsBillingAddress { get; set; }



    public Order Order { get; set; } = null!;
}
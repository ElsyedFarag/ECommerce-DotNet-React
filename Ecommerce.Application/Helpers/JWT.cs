namespace Ecommerce.Application.Helpers;

public class JWT
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public double DurationInDays { get; set; }
}
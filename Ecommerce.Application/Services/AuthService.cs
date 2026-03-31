using Ecommerce.Application.Dtos.Auth;
using Ecommerce.Application.Helpers;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Application.Services;

public class AuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly CustomerService _customerService;
    private readonly JWT _jwt;

    public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, 
        CustomerService customerService)
    {
        _userManager = userManager;
        _jwt = jwt.Value;
        _customerService = customerService;
    }

    public async Task<AuthDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user is null)
            throw new Exception("User not found");

        var isChecked = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isChecked)
            throw new Exception("Email or password is correct!");


        var roles = await _userManager.GetRolesAsync(user);
        var jwtSecurityToken = await CreateJwtToken(user);


        return new AuthDto
        {
            Email = user.Email!,
            Username = user.UserName!,
            ExpiresOn = jwtSecurityToken.ValidTo,
            Roles = roles.ToList(),
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
        };


    }

    public async Task<AuthDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
            throw new Exception($"This email: {registerDto.Email} is already registered!");

        if (await _userManager.FindByNameAsync(registerDto.Username) is not null)
            throw new Exception($"This username: {registerDto.Username} is already registered!");

        var user = new ApplicationUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Username,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception(errors);
        }

        await _userManager.AddToRoleAsync(user, "Customer");
        await _customerService.CreateCustomerAsync(new Customer
        {
            UserId = user.Id,
        });

        var roles = await _userManager.GetRolesAsync(user);

        var jwtSecurityToken = await CreateJwtToken(user);

        return new AuthDto
        {

            Username = registerDto.Username,
            Email = registerDto.Email,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            ExpiresOn = DateTime.UtcNow,
            Roles = roles.ToList(),
        };
    }


    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("uid", user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
}

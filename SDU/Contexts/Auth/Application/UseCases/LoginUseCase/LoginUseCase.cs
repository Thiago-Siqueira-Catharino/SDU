using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Application.UseCases.LoginUseCase;

using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

public class LoginUseCase(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IConfiguration configuration
    )
{
    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Role, user.GetType().Name),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> Run(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.email);

        if (user == null)
            throw new ArgumentException("Invalid login attempt");

        var validatePassword = await userManager.CheckPasswordAsync(user, loginDto.password);
        if (validatePassword == false)
            throw new ArgumentException("Invalid login attempt");
        
        return GenerateJwtToken(user);
    }
}
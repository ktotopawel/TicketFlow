using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TicketFlow.Api.Entities;

namespace TicketFlow.Api.Services;

public class TokenService(IConfiguration config, ILogger<TokenService> logger)
{
    public string GenerateToken(Guid userId, string email, Role role)
    {
        var jwtSettings = config.GetSection("JwtSettings");
        var jwtSecret = Encoding.UTF8.GetBytes(jwtSettings["Secret"] ??
                                               throw new InvalidOperationException("JWT secret is not configured."));
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var key = new SymmetricSecurityKey(jwtSecret);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationMinutes"] ?? "60")),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
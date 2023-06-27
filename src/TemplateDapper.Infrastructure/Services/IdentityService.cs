using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TemplateDapper.Application.Interfaces;
using TemplateDapper.Domain.ValueObjects;

namespace TemplateDapper.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly IConfiguration _configuration;

    public IdentityService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GenerateToken(CurrentUser request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
        var tokenLifeTime = Convert.ToUInt32(_configuration["JwtSettings:LifeTime"]);
        var tokenIssuer = _configuration["JwtSettings:Issuer"];
        var tokenAudience = _configuration["JwtSettings:Audience"];

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, request.Id.ToString()),
            new (JwtRegisteredClaimNames.Sub, request.Email),
            new (JwtRegisteredClaimNames.Email, request.Email),
            new ("id", request.Id.ToString()),
            new ("name", request.Name),
            new (ClaimTypes.Role, request.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(tokenLifeTime),
            Issuer = tokenIssuer,
            Audience = tokenAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var jwt = tokenHandler.WriteToken(token);
        return await Task.FromResult(jwt);
    }

    public async Task<CurrentUser> GetCurrentUser(string token)
    {
        var authToken = token.Substring(7);
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken = handler.ReadJwtToken(authToken);

        var claims = new Dictionary<string, string>();

        foreach (Claim claim in jwtToken.Claims)
        {
            claims.Add(claim.Type, claim.Value);
        }

        var currentUser = new CurrentUser()
        {
            Id = Convert.ToInt32(claims["id"]),
            Email = claims["email"],
            Name = claims["name"],
            Role = claims["role"]
        };

        return await Task.FromResult(currentUser);
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Infrastructure.Contexts;
using PetFamily.Core.Models;
using PetFamily.Core.Options;

namespace PetFamily.Accounts.Infrastructure.Providers;

public class JwtTokenProvider : ITokenProvider
{
    private readonly AccountsDbContext _context;
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> options, AccountsDbContext accountsDbContext)
    {
        _context = accountsDbContext;
        _jwtOptions = options.Value;
    }
    public  async Task<string> GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roleClaims = await _context.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == user.Id)
            .SelectMany(u => u.Roles)
            .Select(r => new Claim(ClaimTypes.Role, r.Name ?? string.Empty))
            .ToListAsync();
        
        Claim[] claims = [
            new Claim(CustomClaims.Id, user.Id.ToString()),
            new Claim(CustomClaims.Email, user.Email ?? "")
        ];
         
        claims = claims.Concat(roleClaims).ToArray();
        
        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtOptions.ExpiredMinutesTime)),
            signingCredentials: signingCredentials,
            claims: claims);
        
        var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return stringToken;
    }
}
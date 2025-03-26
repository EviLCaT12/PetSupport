using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Infrastructure.Contexts;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Core.Options;
using PetFamily.Framework;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Accounts.Infrastructure.Providers;

public class JwtTokenProvider : ITokenProvider
{
    private readonly AccountsDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(
        IOptions<JwtOptions> options, 
        AccountsDbContext accountsDbContext,
        [FromKeyedServices(ModuleKey.Account)] IUnitOfWork unitOfWork)
    {
        _context = accountsDbContext;
        _unitOfWork = unitOfWork;
        _jwtOptions = options.Value;
    }
    public  async Task<JwtTokenResult> GenerateAccessToken(User user, CancellationToken cancellationToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roleClaims = await _context.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == user.Id)
            .SelectMany(u => u.Roles)
            .Select(r => new Claim(ClaimTypes.Role, r.Name ?? string.Empty))
            .ToListAsync(cancellationToken);

        var jti = Guid.NewGuid();
        
        Claim[] claims = [
            new Claim(CustomClaims.Id, user.Id.ToString()),
            new Claim(CustomClaims.Jti, jti.ToString()),
            new Claim(CustomClaims.Email, user.Email ?? "")
        ];
         
        claims = claims.Concat(roleClaims).ToArray();
        
        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtOptions.ExpiredMinutesTime)),
            signingCredentials: signingCredentials,
            claims: claims);
        
        var jwtStringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return new JwtTokenResult(jwtStringToken, jti);
    }

    public async Task<Guid> GenerateRefreshToken(User user, Guid accessTokenJti, CancellationToken cancellationToken) 
    {
        var refreshSession = new RefreshSession
        {
            User = user,
            CreatedOn = DateTime.UtcNow,
            ExpiresIn = DateTime.UtcNow.AddDays(30),
            Jti = accessTokenJti,
            RefreshToken = Guid.NewGuid()
        };
        
        await _context.RefreshSessions.AddAsync(refreshSession, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return refreshSession.RefreshToken;
    }

    public async Task<Result<IReadOnlyList<Claim>, ErrorList>> GetUserClaims(
        string jwtToken, CancellationToken cancellationToken)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        
        var validationParameters = TokenValidationParametersFactory.CreateWithoutLifeTime(_jwtOptions);

        var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameters);

        if (validationResult.IsValid == false)
            return Errors.Tokens.InvalidToken();

        return validationResult.ClaimsIdentity.Claims.ToList();
    }

}
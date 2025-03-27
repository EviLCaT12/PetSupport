using System.Security.Claims;
using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Accounts.Application;

public interface ITokenProvider
{
    Task<JwtTokenResult> GenerateAccessToken(User user, CancellationToken cancellationToken);

    Task<Result<IReadOnlyList<Claim>, ErrorList>> GetUserClaims(
        string jwtToken, CancellationToken cancellationToken);
    Task<Guid> GenerateRefreshToken(User user, Guid accessTokenJti, CancellationToken cancellationToken);
}
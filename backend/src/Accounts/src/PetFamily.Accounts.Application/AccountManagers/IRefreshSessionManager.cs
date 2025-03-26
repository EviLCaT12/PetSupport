using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Accounts.Application.AccountManagers;

public interface IRefreshSessionManager
{
    Task<Result<RefreshSession, ErrorList>> GetByRefreshToken(
        Guid refreshToken, CancellationToken cancellationToken);

    void Delete(RefreshSession refreshSession);
}
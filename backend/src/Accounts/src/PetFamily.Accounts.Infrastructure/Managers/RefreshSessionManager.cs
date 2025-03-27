using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.AccountManagers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Infrastructure.Contexts;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Accounts.Infrastructure.Managers;

public class RefreshSessionManager(AccountsDbContext context) : IRefreshSessionManager
{
    public async Task<Result<RefreshSession, ErrorList>> GetByRefreshToken(
        Guid refreshToken, CancellationToken cancellationToken)
    {
        var result = await context.RefreshSessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(rs => rs.RefreshToken == refreshToken, cancellationToken);

        if (result is null)
            return Errors.General.ValueNotFound().ToErrorList();

        return result;
    }
    
    public void Delete(RefreshSession refreshSession)
    {
        context.RefreshSessions.Remove(refreshSession);
    }
}

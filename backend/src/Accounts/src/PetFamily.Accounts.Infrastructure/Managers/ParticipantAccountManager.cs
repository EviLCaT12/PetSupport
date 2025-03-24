using PetFamily.Accounts.Application.AccountManagers;
using PetFamily.Accounts.Domain.Entitues.AccountEntitites;
using PetFamily.Accounts.Infrastructure.Contexts;

namespace PetFamily.Accounts.Infrastructure.Managers;

public class ParticipantAccountManager(AccountsDbContext context) : IParticipantAccountManager
{
    public async Task CreateParticipantAccountAsync(ParticipantAccount participantAccount)
    {
        await context.ParticipantAccounts.AddAsync(participantAccount);
        await context.SaveChangesAsync();
    }
}
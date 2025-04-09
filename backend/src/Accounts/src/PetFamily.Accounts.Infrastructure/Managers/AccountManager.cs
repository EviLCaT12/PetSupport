using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.AccountManagers;
using PetFamily.Accounts.Domain.Entities.AccountEntitites;
using PetFamily.Accounts.Infrastructure.Contexts;

namespace PetFamily.Accounts.Infrastructure.Managers;

public class AccountManager(WriteAccountsDbContext context) : IAccountManager
{
    public async Task CreateParticipantAccountAsync(ParticipantAccount participantAccount)
    {
        await context.ParticipantAccounts.AddAsync(participantAccount);
    }
    
    public void DeleteParticipantAccountAsync(ParticipantAccount participantAccount)
    {
        context.ParticipantAccounts.Remove(participantAccount);
    }

    public async Task<ParticipantAccount?> GetParticipantAccountByUserIdAsync(Guid userId)
    {
        var account = await context.ParticipantAccounts.FirstOrDefaultAsync(a => a.User.Id == userId);
        return account;
    }

    public async Task CreateVolunteerAccountAsync(VolunteerAccount volunteerAccount, CancellationToken cancellationToken)
    {
        await context.VolunteerAccounts.AddAsync(volunteerAccount, cancellationToken);
    }

    public async Task CreateAdminAccountAsync(AdminAccount adminAccount)
    {
        await context.AdminAccounts.AddAsync(adminAccount);
    }
}
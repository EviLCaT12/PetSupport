using PetFamily.Accounts.Application.AccountManagers;
using PetFamily.Accounts.Domain.Entities.AccountEntitites;
using PetFamily.Accounts.Infrastructure.Contexts;

namespace PetFamily.Accounts.Infrastructure.Managers;

public class AccountManager(AccountsDbContext context) : IAccountManager
{
    public async Task CreateParticipantAccountAsync(ParticipantAccount participantAccount)
    {
        await context.ParticipantAccounts.AddAsync(participantAccount);
        await context.SaveChangesAsync();
    }

    public async Task CreateVolunteerAccountAsync(VolunteerAccount volunteerAccount)
    {
        await context.VolunteerAccounts.AddAsync(volunteerAccount);
        await context.SaveChangesAsync();
    }

    public async Task CreateAdminAccountAsync(AdminAccount adminAccount)
    {
        await context.AdminAccounts.AddAsync(adminAccount);
        await context.SaveChangesAsync();
    }
}
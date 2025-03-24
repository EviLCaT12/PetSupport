using PetFamily.Accounts.Domain.Entitues.AccountEntitites;
using PetFamily.Accounts.Infrastructure.Contexts;

namespace PetFamily.Accounts.Infrastructure.Managers;

public  class AdminAccountManager(AccountsDbContext context)
{
    public async Task CreateAdminAccount(AdminAccount adminAccount)
    {
        await context.AdminAccounts.AddAsync(adminAccount);
        await context.SaveChangesAsync();
    }
}
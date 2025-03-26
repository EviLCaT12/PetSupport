using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Application.AccountManagers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Domain.Entities.AccountEntitites;
using PetFamily.Accounts.Infrastructure.Configurations;
using PetFamily.Accounts.Infrastructure.Managers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeederService(
    RoleManager<Role> roleManager,
    UserManager<User> userManager,
    PermissionManager permissionManager,
    RolePermissionManager rolePermissionManager,
    IAccountManager accountManager,
    IOptions<AdminOptions> adminOptions,
    ILogger<AccountsSeederService> logger,
    [FromKeyedServices(ModuleKey.Account)] IUnitOfWork unitOfWork)
{
    private readonly AdminOptions _adminOptions = adminOptions.Value;
    
    public async Task SeedAsync()
    {
        var json = await File.ReadAllTextAsync(FilePaths.Accounts);
        
        var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json)
                       ?? throw new ApplicationException("Could not deserialize role permission config.");
        
        await SeedPermissions(seedData);

        await SeedRoles(seedData);

        await SeedRolePermissions(seedData);

        await SeedAdminAccount();
    }

    private async Task SeedAdminAccount()
    {
        var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var isAdminExist = await userManager.FindByEmailAsync(adminOptions.Value.Email);
            if (isAdminExist is not null)
                return;
        
            var adminRole = await roleManager.FindByNameAsync(AdminAccount.Admin)
                            ?? throw new ApplicationException("Could not find admin role");

            var fio = Fio.Create(
                _adminOptions.UserName,
                _adminOptions.UserName, 
                _adminOptions.UserName).Value;
        
            var adminUser = User.CreateAdmin(
                _adminOptions.UserName,
                _adminOptions.Email,
                fio,
                adminRole).Value;
        
            await userManager.CreateAsync(adminUser, _adminOptions.Password);

            var adminAccount = new AdminAccount(adminUser);

            await accountManager.CreateAdminAccountAsync(adminAccount);

            await unitOfWork.SaveChangesAsync();
            transaction.Commit();
        
            logger.LogInformation("Admin account created");
        }
        catch (Exception e)
        {
            transaction.Rollback();
            logger.LogError(e, "Failed to create admin account");
            throw;
        }
        

    }
    
    private async Task SeedRolePermissions(RolePermissionConfig seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            var seedDataRole = seedData.Roles[roleName];
            
            await rolePermissionManager.AddRangeIfExist(role!.Id, seedDataRole);
        }
        logger.LogInformation("Seeding role permissions to database.");
    }

    private  async Task SeedPermissions(RolePermissionConfig seedData)
    {
        var permissionsToAdd = seedData.Permissions
            .SelectMany(permissionGroup => permissionGroup.Value);
        
        await permissionManager.AddRangeIfExist(permissionsToAdd);
        
        logger.LogInformation("Permissions added to database.");
    }

    private async Task SeedRoles(RolePermissionConfig seedData)
    {
        foreach (var role in seedData.Roles.Keys)
        {
            var isRoleExist = await roleManager.FindByNameAsync(role);
            
            if (isRoleExist != null)
                continue;

            await roleManager.CreateAsync(new Role { Name = role });

        }
        
        logger.LogInformation("Roles added to database.");
    }
}
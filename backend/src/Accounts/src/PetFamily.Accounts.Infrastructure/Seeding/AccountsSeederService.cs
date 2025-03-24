using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Domain.Entitues;
using PetFamily.Accounts.Domain.Entitues.AccountEntitites;
using PetFamily.Accounts.Infrastructure.Configurations;
using PetFamily.Accounts.Infrastructure.Managers;
using PetFamily.Framework;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeederService(
    RoleManager<Role> roleManager,
    UserManager<User> userManager,
    PermissionManager permissionManager,
    RolePermissionManager rolePermissionManager,
    AdminAccountManager adminAccountManager,
    IOptions<AdminOptions> adminOptions,
    ILogger<AccountsSeederService> logger)
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

        var adminRole = await roleManager.FindByNameAsync(AdminAccount.ADMIN)
                        ?? throw new ApplicationException("Could not find admin role");

        var adminUser = User.CreateAdmin(_adminOptions.UserName, _adminOptions.Email, adminRole).Value;

        var adminAccount = new AdminAccount(adminUser);

        await adminAccountManager.CreateAdminAccount(adminAccount);
    }
    
    private  async Task SeedRolePermissions(RolePermissionConfig seedData)
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
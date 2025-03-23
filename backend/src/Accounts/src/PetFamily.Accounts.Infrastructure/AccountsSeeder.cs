using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.Entitues;
using PetFamily.Accounts.Infrastructure.Configurations;
using PetFamily.Accounts.Infrastructure.Contexts;
using PetFamily.Accounts.Infrastructure.Managers;
using PetFamily.Framework;

namespace PetFamily.Accounts.Infrastructure;

public class AccountsSeeder
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<AccountsSeeder> _logger;

    public AccountsSeeder(IServiceScopeFactory serviceScopeFactory, ILogger<AccountsSeeder> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding accounts...");
        
        var json = await File.ReadAllTextAsync(FilePaths.Accounts);
        
        _logger.LogInformation($"Loaded {json} accounts.");
        
        using var scope = _serviceScopeFactory.CreateScope();
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        var permissionManager = scope.ServiceProvider.GetRequiredService<PermissionManager>();
        
        var rolePermissionManager = scope.ServiceProvider.GetRequiredService<RolePermissionManager>();

        var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json)
            ?? throw new ApplicationException("Could not deserialize role permission config.");


        await SeedPermissions(seedData, permissionManager);

        await SeedRoles(seedData, roleManager);

        await SeedRolePermissions(seedData, roleManager, rolePermissionManager);
    }

    private  async Task SeedRolePermissions(RolePermissionConfig seedData, RoleManager<Role> roleManager,
        RolePermissionManager rolePermissionManager)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            var seedDataRole = seedData.Roles[roleName];
            
            await rolePermissionManager.AddRangeIfExist(role!.Id, seedDataRole);
        }
        _logger.LogInformation("Seeding role permissions to database.");
    }

    private  async Task SeedPermissions(RolePermissionConfig seedData, PermissionManager permissionManager)
    {
        var permissionsToAdd = seedData.Permissions
            .SelectMany(permissionGroup => permissionGroup.Value);
        
        await permissionManager.AddRangeIfExist(permissionsToAdd);
        
        _logger.LogInformation("Permissions added to database.");
    }

    private async Task SeedRoles(RolePermissionConfig seedData, RoleManager<Role> roleManager)
    {
        foreach (var role in seedData.Roles.Keys)
        {
            var isRoleExist = await roleManager.FindByNameAsync(role);
            
            if (isRoleExist != null)
                continue;

            await roleManager.CreateAsync(new Role { Name = role });

        }
        
        _logger.LogInformation("Roles added to database.");
    }
}
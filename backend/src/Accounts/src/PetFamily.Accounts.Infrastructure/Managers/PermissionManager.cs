using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Domain.Entitues;
using PetFamily.Accounts.Infrastructure.Contexts;

namespace PetFamily.Accounts.Infrastructure.Managers;

public class PermissionManager(AccountsDbContext context)
{
    public async Task<Permission?> FindByCode(string code)
    {
        return await context.Permissions.FirstOrDefaultAsync(p => p.Code == code);
    }
    
    public async Task AddRangeIfExist(IEnumerable<string> permissionsToAdd)
    {
        foreach (var permissionCode in permissionsToAdd)
        {
            var isPermissionExisted = await context.Permissions.AnyAsync(p => p.Code == permissionCode);
            
            if (isPermissionExisted)
                return;

            await context.Permissions.AddAsync(new Permission {Code = permissionCode});
        }
        
        await context.SaveChangesAsync();
        
    }

    public async Task<HashSet<string>> GetUserPermissionCodes(Guid userId)
    {
        var permissions = await context.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .ToListAsync();

        return permissions.ToHashSet();
    }
}
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain.Entitues;
using PetFamily.Accounts.Infrastructure.Contexts;

namespace PetFamily.Accounts.Infrastructure.Managers;

public class RolePermissionManager(AccountsDbContext context)
{
    
    public async Task AddRangeIfExist(Guid roleId, IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var permission = await context.Permissions.FirstOrDefaultAsync(p => p.Code == permissionCode);
                
            var ifRolePermissionExist = await context.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id);

            if (ifRolePermissionExist)
                continue;

            await context.RolePermissions
                .AddAsync(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permission!.Id
                });
        }
        
        await context.SaveChangesAsync();
    }
}
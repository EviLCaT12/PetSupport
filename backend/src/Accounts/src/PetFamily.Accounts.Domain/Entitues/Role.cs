using Microsoft.AspNetCore.Identity;

namespace PetFamily.Accounts.Domain.Entitues;

public class Role : IdentityRole<Guid>
{
    public List<RolePermission> RolePermissions { get; set; }
}
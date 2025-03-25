using Microsoft.AspNetCore.Identity;

namespace PetFamily.Accounts.Domain.Entitues;

public class Role : IdentityRole<Guid>
{
    public List<User> Users { get; set; }
    public List<RolePermission> RolePermissions { get; set; }
}
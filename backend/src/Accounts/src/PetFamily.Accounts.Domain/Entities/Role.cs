using Microsoft.AspNetCore.Identity;

namespace PetFamily.Accounts.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public List<RolePermission> RolePermissions { get; set; }
}
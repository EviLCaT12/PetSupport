namespace PetFamily.Accounts.Infrastructure.Configurations.Write;

public class RolePermissionConfig
{
    public Dictionary<string, string[]> Permissions { get; set; }
    public Dictionary<string, string[]> Roles { get; set; }
}
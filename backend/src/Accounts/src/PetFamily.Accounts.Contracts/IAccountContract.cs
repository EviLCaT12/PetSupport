namespace PetFamily.Accounts.Contracts;

public interface IAccountContract
{
    Task<HashSet<string>> GetUserPermissionCodes(Guid userId);
}
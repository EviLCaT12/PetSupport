namespace PetFamily.Accounts.Contracts;

public interface IAccountContract
{
    Task<HashSet<string>> GetUserPermissionCodes(Guid userId);
    
    Task<bool> IsUserAlreadyVolunteer(Guid userId, CancellationToken cancellationToken = default);
}
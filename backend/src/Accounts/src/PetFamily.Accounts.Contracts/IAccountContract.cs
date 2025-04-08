using CSharpFunctionalExtensions;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Accounts.Contracts;

public interface IAccountContract
{
    Task<HashSet<string>> GetUserPermissionCodes(Guid userId);
    
    Task<bool> IsUserAlreadyVolunteer(Guid userId, CancellationToken cancellationToken = default);
    
    Task<Result<bool, ErrorList>> IsUserCanSendVolunteerRequests(Guid userId,
        CancellationToken cancellationToken = default);

    Task<UnitResult<ErrorList>> BanUserToSendVolunteerRequest(BanUserToSendVolunteerRequestRequest request,
        CancellationToken cancellationToken = default);


}
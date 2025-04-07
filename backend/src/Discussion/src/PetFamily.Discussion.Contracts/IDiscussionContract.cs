using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace Contracts;

public interface IDiscussionContract
{
    Result<Guid, ErrorList> CreateDiscussionForVolunteerRequest(Guid requestId, IEnumerable<Guid> membersId);
}
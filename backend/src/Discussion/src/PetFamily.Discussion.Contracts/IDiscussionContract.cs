using Contracts.Requests;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace Contracts;

public interface IDiscussionContract
{
    Task<Result<Guid, ErrorList>> CreateDiscussionForVolunteerRequest(CreateDiscussionRequest request,
        CancellationToken cancellationToken = default);
}
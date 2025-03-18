using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.CreateVolunteer;

namespace PetFamily.Volunteers.Contracts;

public interface IVolunteerContract
{
    Task<Result<Guid, ErrorList>> AddVolunteer(CreateVolunteerRequest request, CancellationToken cancellation = default);
}
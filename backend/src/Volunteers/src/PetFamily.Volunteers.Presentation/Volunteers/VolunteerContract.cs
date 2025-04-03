using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.Volunteers.Application.Commands.Create;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.CreateVolunteer;

namespace PetFamily.Volunteer.Api.Volunteers;

public class VolunteerContract : IVolunteerContract
{
    private readonly CreateVolunteerHandler _handler;

    public VolunteerContract(CreateVolunteerHandler handler)
    {
        _handler = handler;
    }

    public async Task<Result<Guid, ErrorList>> AddVolunteer(
        CreateVolunteerRequest request, CancellationToken cancellation = default)
    {
        var volunteer = await _handler.HandleAsync(
            new CreateVolunteerCommand(
                request.Fio,
                request.PhoneNumber,
                request.Email,
                request.Description),
            cancellation);

        if (volunteer.IsFailure)
            return volunteer.Error;

        return volunteer.Value;
    }
}
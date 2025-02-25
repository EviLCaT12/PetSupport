using PetFamily.Application.Dto.VolunteerDto;
using PetFamily.Application.PetManagement.Commands.UpdateMainInfo;

namespace PetFamily.API.Requests.Volunteers.UpdateVolunteer;

public record UpdateVolunteerMainInfoRequest(
    FioDto Fio,
    string Phone,
    string Email,
    string Description,
    int YearsOfExperience)
{
    public UpdateVolunteerMainInfoCommand ToCommand(Guid volunteerId)
        => new UpdateVolunteerMainInfoCommand(
            volunteerId,
            Fio,
            Phone,
            Email,
            Description,
            YearsOfExperience);
}
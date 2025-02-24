using PetFamily.Application.Dto.VolunteerDto;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Requests.UpdateVolunteer;

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
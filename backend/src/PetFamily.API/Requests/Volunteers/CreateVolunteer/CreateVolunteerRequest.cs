using PetFamily.Application.Dto.Shared;
using PetFamily.Application.Dto.VolunteerDto;
using PetFamily.Application.PetManagement.Commands.Create;

namespace PetFamily.API.Requests.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    FioDto Fio,
    string PhoneNumber,
    string Email,
    string Description,
    int YearsOfExperience,
    List<SocialWebDto> SocialWebDto,
    List<TransferDetailDto> TransferDetailDto
)
{
    public CreateVolunteerCommand ToCommand()
        => new CreateVolunteerCommand(
            Fio,
            PhoneNumber,
            Email,
            Description,
            YearsOfExperience,
            SocialWebDto,
            TransferDetailDto);
}
    
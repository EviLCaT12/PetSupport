using PetFamily.Core.Dto.Shared;
using PetFamily.Core.Dto.VolunteerDto;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteer.CreateVolunteer;

public record CreateVolunteerRequest(
    FioDto Fio,
    string PhoneNumber,
    string Email,
    string Description,
    int YearsOfExperience,
    List<SocialWebDto> SocialWebDto,
    List<TransferDetailDto> TransferDetailDto
);
    
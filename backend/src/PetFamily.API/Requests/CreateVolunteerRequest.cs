
using PetFamily.Application.Dto.Shared;
using PetFamily.Application.Dto.VolunteerDto;

namespace PetFamily.API.Requests;

public record CreateVolunteerRequest(
    FioDto Fio,
    string PhoneNumber, // подумать над объединением телефона и почты в Dto Contacts
    string Email,
    string Description,
    int YearsOfExperience,
    List<SocialWebDto> SocialWebDto,
    List<TransferDetailDto> TransferDetailDto
    );
    
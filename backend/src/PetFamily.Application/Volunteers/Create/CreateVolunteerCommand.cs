using PetFamily.Application.Dto.VolunteerDto;

namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerCommand(
    FioDto Fio,
    string PhoneNumber, 
    string Email,
    string Description,
    int YearsOfExperience);
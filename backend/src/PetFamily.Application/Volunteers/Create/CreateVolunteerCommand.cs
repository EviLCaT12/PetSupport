using PetFamily.Application.Dto.VolunteerDto;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerCommand(
    FioDto Fio,
    string PhoneNumber, 
    string Email,
    string Description,
    int YearsOfExperience);
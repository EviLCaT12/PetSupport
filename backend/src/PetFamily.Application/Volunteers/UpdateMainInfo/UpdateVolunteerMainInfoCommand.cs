using PetFamily.Application.Dto.VolunteerDto;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(
    Guid VolunteerId,
    FioDto Fio, 
    string Phone,
    string Email,
    string Description,
    int YearsOfExperience);
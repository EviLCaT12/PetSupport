using PetFamily.Application.Dto.VolunteerDto;

namespace PetFamily.Application.PetManagement.UseCases.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(
    Guid VolunteerId,
    FioDto Fio, 
    string Phone,
    string Email,
    string Description,
    int YearsOfExperience);
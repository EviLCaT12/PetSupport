using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.VolunteerDto;

namespace PetFamily.Volunteers.Application.Commands.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(
    Guid VolunteerId,
    FioDto Fio, 
    string Phone,
    string Email,
    string Description,
    int YearsOfExperience) : ICommand;
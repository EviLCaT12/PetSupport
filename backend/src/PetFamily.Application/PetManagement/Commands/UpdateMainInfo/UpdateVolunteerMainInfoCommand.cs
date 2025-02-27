using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto.VolunteerDto;

namespace PetFamily.Application.PetManagement.Commands.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(
    Guid VolunteerId,
    FioDto Fio, 
    string Phone,
    string Email,
    string Description,
    int YearsOfExperience) : ICommand;
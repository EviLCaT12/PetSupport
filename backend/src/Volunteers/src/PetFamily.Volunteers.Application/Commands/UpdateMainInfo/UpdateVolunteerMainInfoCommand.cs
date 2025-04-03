using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

namespace PetFamily.Volunteers.Application.Commands.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(
    Guid VolunteerId,
    FioDto Fio, 
    string Phone,
    string Email,
    string Description,
    int YearsOfExperience) : ICommand;
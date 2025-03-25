using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;
using PetFamily.Core.Dto.VolunteerDto;

namespace PetFamily.Volunteers.Application.Commands.Create;

public record CreateVolunteerCommand(
    FioDto Fio,
    string PhoneNumber, 
    string Email,
    string Description) : ICommand;
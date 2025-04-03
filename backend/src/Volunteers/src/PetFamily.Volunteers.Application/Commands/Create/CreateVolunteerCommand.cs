using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

namespace PetFamily.Volunteers.Application.Commands.Create;

public record CreateVolunteerCommand(
    FioDto Fio,
    string PhoneNumber, 
    string Email,
    string Description) : ICommand;
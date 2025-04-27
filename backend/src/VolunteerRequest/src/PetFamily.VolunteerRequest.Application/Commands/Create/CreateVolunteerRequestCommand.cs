using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;

namespace PetFamily.VolunteerRequest.Application.Commands.Create;

public record CreateVolunteerRequestCommand(
    Guid UserId,
    FioDto FullName,
    string Description,
    string Email,
    int Experience) : ICommand;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;

namespace PetFamily.VolunteerRequest.Application.Commands.EditRequest;

public record EditCommand(
    Guid RequestId,
    FioDto Fio,
    string Description,
    string Email,
    int Experience) : ICommand; 
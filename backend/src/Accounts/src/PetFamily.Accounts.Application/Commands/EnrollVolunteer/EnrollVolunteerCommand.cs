using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.EnrollVolunteer;

public record EnrollVolunteerCommand(
    Guid UserId,
    int Experience,
    string PhoneNumber,
    string Description
    ) : ICommand;
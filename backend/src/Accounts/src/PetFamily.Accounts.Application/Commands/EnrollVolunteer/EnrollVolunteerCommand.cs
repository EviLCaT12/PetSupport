using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.EnrollVolunteer;

public record EnrollVolunteerCommand(
    string Email,
    int Experience,
    string PhoneNumber,
    string Description
    ) : ICommand;
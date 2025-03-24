using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.CreateVolunteerAccount;

public record CreateVolunteerAccountCommand(
    string Username,
    string Email,
    string Password,
    int Experience) : ICommand;
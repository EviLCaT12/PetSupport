using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.VolunteerDto;

namespace PetFamily.Accounts.Application.Commands.CreateVolunteerAccount;

public record CreateVolunteerAccountCommand(
    string Username,
    string Email,
    FioDto Fio,
    string Password,
    int Experience) : ICommand;
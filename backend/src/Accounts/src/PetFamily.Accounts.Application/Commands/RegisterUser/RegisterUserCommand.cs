using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.VolunteerDto;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email, 
    string UserName, 
    FioDto Fio,
    string Password) : ICommand;
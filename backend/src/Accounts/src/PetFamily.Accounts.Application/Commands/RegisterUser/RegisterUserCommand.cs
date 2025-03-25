using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.VolunteerDto;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email, 
    string UserName, 
    string Password) : ICommand;
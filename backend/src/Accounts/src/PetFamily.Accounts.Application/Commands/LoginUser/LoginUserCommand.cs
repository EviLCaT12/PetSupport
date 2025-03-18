
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : ICommand;
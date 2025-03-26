using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Accounts.Application.Commands.LoginUser;

public class LoginUserHandler : ICommandHandler<string, LoginUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<LoginUserHandler> _logger;

    public LoginUserHandler(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        ILogger<LoginUserHandler> logger)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }
    public async Task<Result<string, ErrorList>> HandleAsync(
        LoginUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            return Errors.General.ValueNotFound().ToErrorList();
        }

        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!passwordConfirmed)
            return Errors.User.InvalidCredentials().ToErrorList();

        var token =  await _tokenProvider.GenerateAccessToken(user);
        
        _logger.LogInformation("User {Email} logged in.", user.Email);
        
        return token;
    }
}
using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.CreateVolunteerAccount;
using PetFamily.Accounts.Application.Commands.LoginUser;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Framework;

namespace PetFamily.Accounts.Presentation;
    
public class AccountController : ApplicationController
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.UserName,
            request.Password);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(
            request.Email,
            request.Password);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpPost("registration/volunteer")]
    public async Task<IActionResult> RegisterVolunteer(
        [FromBody] CreateVolunteerAccountRequest request,
        [FromServices] CreateVolunteerAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateVolunteerAccountCommand(
            request.UserName,
            request.Email,
            request.Password,
            request.Experience);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
}
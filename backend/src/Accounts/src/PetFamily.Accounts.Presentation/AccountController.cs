using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.EnrollVolunteer;
using PetFamily.Accounts.Application.Commands.LoginUser;
using PetFamily.Accounts.Application.Commands.RefreshTokens;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Accounts.Application.Queries.GerUserById;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;

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
            request.Fio,
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
    
    [Authorize(Permissions.Volunteers.CreateVolunteer)]
    [HttpPost("registration/{userId:guid}/volunteer")]
    public async Task<IActionResult> RegisterVolunteer(
        [FromRoute] Guid userId,
        [FromBody] CreateVolunteerAccountRequest request,
        [FromServices] EnrollVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new EnrollVolunteerCommand(
            userId,
            request.Experience,
            request.PhoneNumber,
            request.Description);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens(
        [FromBody] RefreshTokenRequest request,
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokensCommand(request.AccessToken, request.RefreshToken);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserById(
        [FromRoute] Guid userId,
        [FromServices] GetUserByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(userId);
        
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}
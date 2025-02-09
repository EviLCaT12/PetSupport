using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Requests;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateVolunteerCommand(
            request.Fio, 
            request.PhoneNumber, 
            request.Email, 
            request.Description,
            request.YearsOfExperience);
        
        var result = await handler.HandleAsync(
            command,
            request.SocialWebDto,
            request.TransferDetailDto,
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return result.Value;
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromServices] UpdateVolunteerMainInfoHandler handler,
        [FromBody] UpdateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateVolunteerMainInfoCommand(
            id,
            request.Fio,
            request.Phone,
            request.Email,
            request.Description, 
            request.YearsOfExperience);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return result.Value; 
    }
    
    
}
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Requests;
using PetFamily.Application.Volunteers.CreateVolunteer;
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
        var result = await handler.HandleAsync(
            request.CreateVolunteerCommand,
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
        [FromServices] UpdateMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateVolunteerCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return result.Value; 
    }
    
    
}
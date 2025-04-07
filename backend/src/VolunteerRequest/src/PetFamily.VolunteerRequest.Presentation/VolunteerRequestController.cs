using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.VolunteerRequest.Application.Commands.Create;
using PetFamily.VolunteerRequest.Application.Commands.TakeRequestOnReview;
using PetFamily.VolunteerRequest.Contracts.Requests;

namespace PetFamily.VolunteerRequest.Presentation;


public class VolunteerRequestController : ApplicationController
{
    [Authorize(Permissions.VolunteerRequests.CreateVolunteerRequest)]
    [HttpPost("{userId:guid}")]
    public async Task<ActionResult> CreateVolunteerRequest(
        [FromRoute] Guid userId,
        [FromBody] CreateVolunteerRequestRequest request,
        [FromServices] CreateVolunteerRequestHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateVolunteerRequestCommand(
            userId,
            request.FullName,
            request.Description,
            request.Email,
            request.Experience);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Authorize(Permissions.VolunteerRequests.TakeRequestOnReview)]
    [HttpPost("{requestId:guid}/{adminId:guid}")]
    public async Task<ActionResult> TakeRequestOnReview(
        [FromRoute] Guid requestId, Guid adminId,
        [FromServices] TakeRequestOnReviewHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new TakeRequestOnReviewCommand(requestId, adminId);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
}
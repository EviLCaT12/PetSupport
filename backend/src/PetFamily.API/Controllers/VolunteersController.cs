using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Requests;
using PetFamily.Application.Volunteers.CreateVolunteer;

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
    
}
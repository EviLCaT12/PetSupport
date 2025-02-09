using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Requests;
using PetFamily.API.Requests.CreateVolunteer;
using PetFamily.API.Requests.UpdateVolunteer;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialWeb;
using PetFamily.Application.Volunteers.UpdateTransferDetails;

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
        [FromBody] UpdateVolunteerMainInfoRequest mainInfoRequest,
        CancellationToken cancellationToken)
    {
        var command = new UpdateVolunteerMainInfoCommand(
            id,
            mainInfoRequest.Fio,
            mainInfoRequest.Phone,
            mainInfoRequest.Email,
            mainInfoRequest.Description, 
            mainInfoRequest.YearsOfExperience);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return result.Value; 
    }

    [HttpPut("{id:guid}/social-web")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromServices] UpdateVolunteerSocialWebHandler handler,
        [FromBody] UpdateVolunteerSocialWebRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateVolunteerSocialWebCommand(id, request.SocialWebs);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return result.Value;
    }
    
    [HttpPut("{id:guid}/transfer-detail")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromServices] UpdateVolunteerTransferDetailsHandler handler,
        [FromBody] UpdateVolunteerTransferDetailsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateVolunteerTransferDetailsCommand(id, request.NewTransferDetail);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return result.Value;
    }
    
}
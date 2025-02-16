using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Requests;
using PetFamily.API.Requests.AddPet;
using PetFamily.API.Requests.CreateVolunteer;
using PetFamily.API.Requests.UpdateVolunteer;
using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.HardDelete;
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
    public async Task<ActionResult<Guid>> UpdateMainInfo(
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
    public async Task<ActionResult<Guid>> UpdateSocialWeb(
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
    public async Task<ActionResult<Guid>> UpdateTransferDetail(
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
    
    [HttpDelete("{id:guid}/hard")]
    public async Task<ActionResult<Guid>> HardDelete(
        [FromRoute] Guid id,
        [FromServices] HardDeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteVolunteerCommand(id);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return result.Value;
    }
    
    [HttpDelete("{id:guid}/soft")]
    public async Task<ActionResult<Guid>> SoftDelete(
        [FromRoute] Guid id,
        [FromServices] SoftDeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteVolunteerCommand(id);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return result.Value;
    }

    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid id,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddPetCommand(
            id,
            request.Name,
            request.Classification,
            request.Description,
            request.Color,
            request.HealthInfo,
            request.Address,
            request.Dimensions,
            request.OwnerPhone,
            request.IsCastrate,
            request.DateOfBirth,
            request.IsVaccinated,
            request.HelpStatus,
            request.TransferDetailsDto);

        var addPetResult = await handler.HandleAsync(command, cancellationToken);
        if (addPetResult.IsFailure)
            return addPetResult.Error.ToResponse();
        
                
        return Ok(addPetResult.Value);
    }
    
}
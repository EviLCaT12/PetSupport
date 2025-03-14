using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.API.Requests.Shared;
using PetFamily.API.Requests.Volunteers.AddPet;
using PetFamily.API.Requests.Volunteers.ChangePetHelpStatus;
using PetFamily.API.Requests.Volunteers.ChangePosition;
using PetFamily.API.Requests.Volunteers.CreateVolunteer;
using PetFamily.API.Requests.Volunteers.PetMainPhoto;
using PetFamily.API.Requests.Volunteers.PetPhotos;
using PetFamily.API.Requests.Volunteers.UpdatePetRequest;
using PetFamily.API.Requests.Volunteers.UpdateVolunteer;
using PetFamily.Application.PetManagement.Commands.AddPet;
using PetFamily.Application.PetManagement.Commands.AddPetPhotos;
using PetFamily.Application.PetManagement.Commands.ChangePetHelpStatus;
using PetFamily.Application.PetManagement.Commands.ChangePetPosition;
using PetFamily.Application.PetManagement.Commands.Create;
using PetFamily.Application.PetManagement.Commands.Delete;
using PetFamily.Application.PetManagement.Commands.DeletePet;
using PetFamily.Application.PetManagement.Commands.DeletePetPhotos;
using PetFamily.Application.PetManagement.Commands.MainPetPhoto;
using PetFamily.Application.PetManagement.Commands.UpdateMainInfo;
using PetFamily.Application.PetManagement.Commands.UpdatePet;
using PetFamily.Application.PetManagement.Commands.UpdateSocialWeb;
using PetFamily.Application.PetManagement.Commands.UpdateTransferDetails;
using PetFamily.Application.PetManagement.Queries.GetVolunteerById;
using PetFamily.Application.PetManagement.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteers;

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
        var command = request.ToCommand();
        
        var result = await handler.HandleAsync(
            command,
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
        var command = mainInfoRequest.ToCommand(id);
        
        var result = await handler.HandleAsync(command, cancellationToken);

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
        var command = request.ToCommand(id);
        
        var result = await handler.HandleAsync(command, cancellationToken);
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
        var command = request.ToCommand(id);
        
        var result = await handler.HandleAsync(command, cancellationToken);
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
        
        var result = await handler.HandleAsync(command, cancellationToken);
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
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return result.Value;
    }

    [HttpPut("{id:guid}/pet")]
    public async Task<ActionResult<Guid>> AddPet(
        [FromRoute] Guid id,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var addPetResult = await handler.HandleAsync(command, cancellationToken);
        if (addPetResult.IsFailure)
            return addPetResult.Error.ToResponse();
        
                
        return Ok(addPetResult.Value);
    }
    
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> DeletePetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] DeletePetPhotoRequest request,
        [FromServices] DeletePetPhotosHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();

        return Ok(); 
    }

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> AddPetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] AddPetPhotosRequest request,
        [FromServices] AddPetPhotosHandler handler,
        CancellationToken cancellationToken)
    {
        await using var processor = new FormFileProcessor();
        
        var createPhotoDtos = processor.Process(request.Photos);

        var command = new AddPetPhotosCommand(volunteerId, petId, createPhotoDtos);

        var resultHandle = await handler.HandleAsync(
            command,
            cancellationToken);
        
        if (resultHandle.IsFailure)
            return resultHandle.Error.ToResponse();

        return Ok();
    }

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/position")]
    public async Task<ActionResult> ChangePetPosition(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] ChangePetPositionRequest request,
        [FromServices] ChangePetPositionHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetEntityWithPaginationRequest request,
        [FromServices] GetVolunteersWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var result = await handler.HandleAsync(query, cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] GetVolunteerByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetVolunteerByIdQuery(id);
        var result = await handler.HandleAsync(query, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}")]
    public async Task<ActionResult<Guid>> UpdatePet(
        [FromRoute] Guid volunteerId, Guid petId,
        [FromBody] UpdatePetRequest request,
        [FromServices] UpdatePetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);

        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        
        return Ok(handleResult.Value);
    }

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/help-status")]
    public async Task<ActionResult> ChangePetStatus(
        [FromRoute] Guid volunteerId, Guid petId,
        [FromBody] ChangePetHelpStatusRequest request,
        [FromServices] ChangePetHelpStatusHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        
        return Ok();
    }

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/soft")]
    public async Task<ActionResult> SoftDeletePet(
        [FromRoute] Guid volunteerId, Guid petId,
        [FromServices] SoftDeletePetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetCommand(volunteerId, petId);
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        return Ok();
    }
    
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/hard")]
    public async Task<ActionResult> HardDeletePet(
        [FromRoute] Guid volunteerId, Guid petId,
        [FromServices] HardDeletePetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetCommand(volunteerId, petId);
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        return Ok();
    }

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/set-main-photo")]
    public async Task<ActionResult> SetPetMainPhoto(
        [FromRoute] Guid volunteerId, Guid petId,
        [FromBody] PetMainPhotoRequest request,
        [FromServices] SetPetMainPhotoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        return Ok();
    }
    
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/remove-main-photo")]
    public async Task<ActionResult> RemovePetMainPhoto(
        [FromRoute] Guid volunteerId, Guid petId,
        [FromBody] PetMainPhotoRequest request,
        [FromServices] RemovePetMainPhotoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        return Ok();
    }
}
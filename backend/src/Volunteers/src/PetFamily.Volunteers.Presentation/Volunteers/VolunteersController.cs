using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteer.Api.Processors;
using PetFamily.Volunteers.Application.Commands.AddPet;
using PetFamily.Volunteers.Application.Commands.AddPetPhotos;
using PetFamily.Volunteers.Application.Commands.ChangePetHelpStatus;
using PetFamily.Volunteers.Application.Commands.ChangePetPosition;
using PetFamily.Volunteers.Application.Commands.Create;
using PetFamily.Volunteers.Application.Commands.Delete;
using PetFamily.Volunteers.Application.Commands.DeletePet;
using PetFamily.Volunteers.Application.Commands.DeletePetPhotos;
using PetFamily.Volunteers.Application.Commands.MainPetPhoto;
using PetFamily.Volunteers.Application.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Application.Commands.UpdatePet;
using PetFamily.Volunteers.Application.Queries.GetVolunteerById;
using PetFamily.Volunteers.Application.Queries.GetVolunteersWithPagination;
using PetFamily.Volunteers.Contracts.Requests.Volunteer;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.AddPet;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.ChangePetHelpStatus;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.ChangePosition;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.CreateVolunteer;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.PetMainPhoto;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.PetPhotos;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.UpdatePetRequest;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.UpdateVolunteer;

namespace PetFamily.Volunteer.Api.Volunteers;

[Authorize]
public class VolunteersController : ApplicationController
{
    [Permission(Permissions.Volunteers.CreateVolunteer)]
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
            request.Description);
        
        var result = await handler.HandleAsync(
            command,
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return result.Value;
    }

    [Permission(Permissions.Volunteers.UpdateMainInfoVolunteer)]
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
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return result.Value; 
    }
    
    [Permission(Permissions.Volunteers.DeleteVolunteer)]
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
    
    [Permission(Permissions.Volunteers.DeleteVolunteer)]
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

    [Permission(Permissions.Volunteers.CreatePet)]
    [HttpPut("{id:guid}/pet")]
    public async Task<ActionResult<Guid>> AddPet(
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
    
    [Permission(Permissions.Volunteers.DeletePetPhoto)]
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> DeletePetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] DeletePetPhotoRequest request,
        [FromServices] DeletePetPhotosHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetPhotosCommand(volunteerId, petId, request.PhotoNames);
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();

        return Ok(); 
    }

    [Permission(Permissions.Volunteers.CreatePetPhoto)]
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

    [Permission(Permissions.Volunteers.ChangePetPosition)]
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/position")]
    public async Task<ActionResult> ChangePetPosition(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] ChangePetPositionRequest request,
        [FromServices] ChangePetPositionHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ChangePetPositionCommand(volunteerId, petId, request.NewPetPosition);
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        
        return Ok();
    }

    [Permission(Permissions.Volunteers.GetVolunteer)]
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetVolunteersWithPaginationRequest request,
        [FromServices] GetVolunteersWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetVolunteerWithPaginationQuery(request.Page, request.PageSize);
        
        var result = await handler.HandleAsync(query, cancellationToken);

        return Ok(result.Value);
    }

    [Permission(Permissions.Volunteers.GetVolunteer)]
    [AllowAnonymous]
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

    [Permission(Permissions.Volunteers.UpdatePet)]
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}")]
    public async Task<ActionResult<Guid>> UpdatePet(
        [FromRoute] Guid volunteerId, Guid petId,
        [FromBody] UpdatePetRequest request,
        [FromServices] UpdatePetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePetCommand(
            volunteerId,
            petId,
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
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);

        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        
        return Ok(handleResult.Value);
    }

    [Permission(Permissions.Volunteers.ChangePetHelpStatus)]
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/help-status")]
    public async Task<ActionResult> ChangePetStatus(
        [FromRoute] Guid volunteerId, Guid petId,
        [FromBody] ChangePetHelpStatusRequest request,
        [FromServices] ChangePetHelpStatusHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ChangePetHelpStatusCommand(volunteerId, petId, request.HelpStatus);
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        
        return Ok();
    }

    [Permission(Permissions.Volunteers.DeletePet)]
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
    
    [Permission(Permissions.Volunteers.DeletePet)]
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

    [Permission(Permissions.Volunteers.SetMainPetPhoto)]
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/set-main-photo")]
    public async Task<ActionResult> SetPetMainPhoto(
        [FromRoute] Guid volunteerId, Guid petId,
        [FromBody] PetMainPhotoRequest request,
        [FromServices] SetPetMainPhotoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new PetMainPhotoCommand(volunteerId, petId, request.PhotoPath);
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        return Ok();
    }
    
    [Permission(Permissions.Volunteers.RemoveMainPetPhoto)]
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/remove-main-photo")]
    public async Task<ActionResult> RemovePetMainPhoto(
        [FromRoute] Guid volunteerId, Guid petId,
        [FromBody] PetMainPhotoRequest request,
        [FromServices] RemovePetMainPhotoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new PetMainPhotoCommand(volunteerId, petId, request.PhotoPath);
        
        var handleResult = await handler.HandleAsync(command, cancellationToken);
        if (handleResult.IsFailure)
            return handleResult.Error.ToResponse();
        return Ok();
    }
}
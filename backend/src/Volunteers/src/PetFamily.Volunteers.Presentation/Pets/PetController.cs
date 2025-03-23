using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteers.Application.Queries.GetPetById;
using PetFamily.Volunteers.Application.Queries.GetPetsWithPagination;
using PetFamily.Volunteers.Contracts.Requests.Pet;

namespace PetFamily.Volunteer.Api.Pets;


public class PetController : ApplicationController
{
    [Permission(Permissions.Volunteers.GetPet)]
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetPetsWithPaginationRequest request,
        [FromServices] GetPetsWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPetsWithPaginationQuery(
            request.VolunteerId,
            request.Name,
            request.PositionFrom,
            request.PositionTo,
            request.SpeciesId,
            request.BreedId,
            request.Color,
            request.City,
            request.Street,
            request.HouseNumber,
            request.HeightFrom,
            request.HeightTo,
            request.WeightFrom,
            request.WeightTo,
            request.IsCastrated,
            request.Younger,
            request.Older,
            request.IsVaccinated,
            request.HelpStatus,
            request.SortBy,
            request.SortDirection,
            request.Page,
            request.PageSize);
        
        var response = await handler.HandleAsync(query, cancellationToken);
        if (response.IsFailure)
            return response.Error.ToResponse();
        
        return Ok(response.Value);
    }
    
    [Permission(Permissions.Volunteers.GetPet)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] GetPetByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPetByIdQuery(id);
        
        var response = await handler.HandleAsync(query, cancellationToken);
        if (response.IsFailure)
            return response.Error.ToResponse();
        
        return Ok(response.Value);
    }
}
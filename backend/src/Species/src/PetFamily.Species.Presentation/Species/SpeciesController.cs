using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Species.Application.Commands.AddBreeds;
using PetFamily.Species.Application.Commands.Create;
using PetFamily.Species.Application.Commands.Remove;
using PetFamily.Species.Application.Commands.RemoveBreed;
using PetFamily.Species.Application.Queries.GetBreedsByIdWithPagination;
using PetFamily.Species.Application.Queries.GetSpeciesWithPagination;
using PetFamily.Species.Contracts.Requests.Species;

namespace PetFamily.Species.Presentation.Species;

[Authorize]
public class SpeciesController : ApplicationController
{
    [Permission(Permissions.Species.CreateSpecies)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateRequest request,
        [FromServices] CreateHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommand(request.Name);
        
        var createResult = await handler.HandleAsync(command, cancellationToken);
        if (createResult.IsFailure)
            return createResult.Error.ToResponse();
        
        return Ok(createResult.Value);
    }
    
    [Permission(Permissions.Species.CreateBreeds)]
    [HttpPost("{speciesId:guid}/breeds")]
    public async Task<ActionResult<IEnumerable<Guid>>> AddBreeds(
        [FromRoute] Guid speciesId,
        [FromBody] AddBreedsRequest request,
        [FromServices] AddBreedsHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddBreedsCommand(speciesId, request.Names);
        
        var addBreedsResult = await handler.HandleAsync(command, cancellationToken);
        if (addBreedsResult.IsFailure)
            return addBreedsResult.Error.ToResponse();
        
        return Ok(addBreedsResult.Value);
    }
    
    [Permission(Permissions.Species.DeleteSpecies)]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> RemoveSpecies(
        [FromRoute] Guid id,
        [FromServices] RemoveSpeciesHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveSpeciesCommand(id);
        
        var removeSpeciesResult = await handler.HandleAsync(command, cancellationToken);
        if (removeSpeciesResult.IsFailure)
            return removeSpeciesResult.Error.ToResponse();
        
        return Ok(removeSpeciesResult.Value);
    }
    
    [Permission(Permissions.Species.DeleteBreeds)]
    [HttpDelete("{speciesId:guid}/breeds/{breedId:guid}")]
    public async Task<ActionResult> RemoveBreeds(
        [FromRoute] Guid speciesId, Guid breedId,
        [FromServices] RemoveBreedHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveBreedCommand(speciesId, breedId);
        
        var removeSpeciesResult = await handler.HandleAsync(command, cancellationToken);
        if (removeSpeciesResult.IsFailure)
            return removeSpeciesResult.Error.ToResponse();
        
        return Ok(removeSpeciesResult.Value);
    }

    [Permission(Permissions.Species.GetSpecies)]
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetSpeciesWithPaginationRequest request,
        [FromServices] GetSpeciesWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetSpeciesWithPaginationQuery(request.Page, request.PageSize);
        
        var result = await handler.HandleAsync(query, cancellationToken);
        
        return Ok(result.Value);
    }
    
    [Permission(Permissions.Species.GetBreeds)]
    [HttpGet("{speciesId:guid}/breeds/")]
    public async Task<ActionResult> GetAllBreeds(
        [FromRoute] Guid speciesId,
        [FromQuery]  GetBreedsByIdWithPaginationRequest request,
        [FromServices] GetBreedsByIdWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetBreedsByIdWithPaginationQuery(speciesId, request.Page, request.PageSize);
        
        var result = await handler.HandleAsync(query, cancellationToken);
        
        return Ok(result.Value);
    }
}
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Requests.Species;
using PetFamily.Application.SpeciesManagement.AddBreeds;
using PetFamily.Application.SpeciesManagement.Create;
using PetFamily.Application.SpeciesManagement.Remove;
using PetFamily.Application.SpeciesManagement.RemoveBreed;

namespace PetFamily.API.Controllers.Species;

[ApiController]
[Route("[controller]")]
public class SpeciesController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateRequest request,
        [FromServices] CreateHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        
        var createResult = await handler.HandleAsync(command, cancellationToken);
        if (createResult.IsFailure)
            return createResult.Error.ToResponse();
        
        return Ok(createResult.Value);
    }

    [HttpPost("{speciesId:guid}/breeds")]
    public async Task<ActionResult<IEnumerable<Guid>>> AddBreeds(
        [FromRoute] Guid speciesId,
        [FromBody] AddBreedsRequest request,
        [FromServices] AddBreedsHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(speciesId);
        
        var addBreedsResult = await handler.HandleAsync(command, cancellationToken);
        if (addBreedsResult.IsFailure)
            return addBreedsResult.Error.ToResponse();
        
        return Ok(addBreedsResult.Value);
    }

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
    
    [HttpDelete("{speciesId:guid}/breeds/{breedId:guid}")]
    public async Task<ActionResult> RemoveSpecies(
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
    
}
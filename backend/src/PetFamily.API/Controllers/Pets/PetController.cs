using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Requests.Pets;
using PetFamily.Application.PetManagement.Queries.GetPetById;
using PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Pets;

[ApiController]
[Route("[controller]")]
public class PetController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetPetsWithPaginationRequest request,
        [FromServices] GetPetsWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var response = await handler.HandleAsync(query, cancellationToken);
        if (response.IsFailure)
            return response.Error.ToResponse();
        
        return Ok(response.Value);
    }
    
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
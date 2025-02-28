using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Requests.Pets;
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
        
        return Ok(response.Value);
    }
}
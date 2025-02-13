using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Minio;
using Minio.AspNetCore;
using Minio.DataModel.Args;
using PetFamily.API.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{ 

    [HttpPost("pet")]
    public async Task<ActionResult> UploadFile(
        IFormFile file,
        [FromServices] AddPetHandler handler, //знаю, что так делать нельзя, сугубо для теста
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        var path = Guid.NewGuid();
        var fileData = new FileData(stream, FilePath.Create(path, "jpg").Value, "photos");

        var result = await handler.Handle([fileData], cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value); 


    }
}
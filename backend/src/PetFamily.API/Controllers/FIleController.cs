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
    public async Task<ActionResult> UploadFiles(
        IFormFile file,
        [FromServices] AddPetHandler handler, //знаю, что так делать нельзя, сугубо для теста
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        var path = Guid.NewGuid();
        var fileData = new FileData(stream, FilePath.Create(path, "jpg").Value, "photos");

        var result = await handler.AddHandle([fileData], cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value); 


    }
    
    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> RemoveFiles(
        [FromRoute] Guid id,
        [FromServices] AddPetHandler handler, //знаю, что так делать нельзя, сугубо для теста
        CancellationToken cancellationToken)
    {
        var path = FilePath.Create(id, "jpg").Value;
        var removeData = new ExistFileData(path, "photos");
        
        var result = await handler.RemoveHandle([removeData], cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value); 
    }
    
    [HttpGet("{id:guid}/pet")]
    public async Task<ActionResult> GetFile(
        [FromRoute] Guid id,
        [FromServices] AddPetHandler handler, //знаю, что так делать нельзя, сугубо для теста
        CancellationToken cancellationToken)
    {
        var path = FilePath.Create(id, "jpg").Value;
        var getData = new ExistFileData(path, "photos");
        
        var result = await handler.GetHandle(getData, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
    
        Console.WriteLine(result.Value);
        return Ok(result.Value); 
        
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Minio;
using Minio.AspNetCore;
using Minio.DataModel.Args;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
    {
        
    }
}
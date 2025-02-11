using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Minio;
using Minio.AspNetCore;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly IMinioClient _minioOptions;

    public FileController(IMinioClient minioOptions)
    {
        _minioOptions = minioOptions;  
    }

    [HttpPost]
    public async Task<ActionResult<string>> UploadFile(IFormFile file)
    {
        return File(file.OpenReadStream(), file.ContentType);
    }
}
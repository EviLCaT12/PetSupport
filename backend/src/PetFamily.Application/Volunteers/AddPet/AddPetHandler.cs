using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Volunteers.AddPet;

//Пока что тестовый, только для работоспособности minio
public class AddPetHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetHandler(
        IFileProvider fileProvider,
        ILogger<AddPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<FilePath, ErrorList>> AddHandle(
        IEnumerable<FileData> fileData,
        CancellationToken cancellationToken)
    {
        var uploadResult = await _fileProvider.UploadFiles(fileData, cancellationToken);
        if (uploadResult.IsFailure)
            return uploadResult.Error;

        return default;
    }
    
    public async Task<Result<FilePath, ErrorList>> RemoveHandle(
        IEnumerable<ExistFileData> fileData,
        CancellationToken cancellationToken)
    {
        var uploadResult = await _fileProvider.RemoveFiles(fileData, cancellationToken);
        if (uploadResult.IsFailure)
            return uploadResult.Error;

        return default;
    }
    
    public async Task<Result<FilePath, ErrorList>> GetHandle(
        ExistFileData fileData,
        CancellationToken cancellationToken)
    {
        var uploadResult = await _fileProvider.GetFilePresignedUrl(fileData, cancellationToken);
        if (uploadResult.IsFailure)
            return uploadResult.Error;

        return default;
    }
}
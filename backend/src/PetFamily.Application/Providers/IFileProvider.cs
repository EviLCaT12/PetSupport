using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<UnitResult<ErrorList>> UploadFilesAsync(
        IEnumerable<FileData> fileData, CancellationToken cancellationToken = default);
    
    Task<Result<IEnumerable<string>, ErrorList>> RemoveFilesAsync(
        IEnumerable<ExistFileData> files, CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetFilePresignedUrl(
        ExistFileData file, CancellationToken cancellationToken);
}
using CSharpFunctionalExtensions;
using PetFamily.Application.Files;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;
using FileInfo = PetFamily.Application.Files.FileInfo;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<UnitResult<ErrorList>> UploadFilesAsync(
        IEnumerable<FileData> fileData, CancellationToken cancellationToken = default);
    
    Task<Result<IEnumerable<string>, ErrorList>> RemoveFilesAsync(
        IEnumerable<FileInfo> files, CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetFilePresignedUrl(
        FileInfo file, CancellationToken cancellationToken);
}
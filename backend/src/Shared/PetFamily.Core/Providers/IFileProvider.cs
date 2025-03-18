using CSharpFunctionalExtensions;
using PetFamily.Core.Files;
using PetFamily.SharedKernel.Error;
using FileInfo = PetFamily.Core.Files.FileInfo;

namespace PetFamily.Core.Providers;

public interface IFileProvider
{
    Task<UnitResult<ErrorList>> UploadFilesAsync(
        IEnumerable<FileData> fileData, CancellationToken cancellationToken = default);
    
    Task<Result<IEnumerable<string>, ErrorList>> RemoveFilesAsync(
        IEnumerable<FileInfo> files, CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetFilePresignedUrl(
        FileInfo file, CancellationToken cancellationToken);
}
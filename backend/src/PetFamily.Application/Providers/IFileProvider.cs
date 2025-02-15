using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, ErrorList>> UploadFiles(
        IEnumerable<FileData> fileData, CancellationToken cancellationToken = default);
    
    Task<Result<IEnumerable<string>, ErrorList>> RemoveFiles(
        IEnumerable<ExistFileData> files, CancellationToken cancellationToken = default);
}
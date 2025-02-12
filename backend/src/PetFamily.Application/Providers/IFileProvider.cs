using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<UnitResult<ErrorList>> UploadFiles(
        FileData fileData, CancellationToken cancellationToken = default);
}
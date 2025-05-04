using CSharpFunctionalExtensions;
using FilesService.Core;
using FilesService.Core.Models;

namespace FilesService.MongoDataAccess;

public interface IFileRepository
{
    Task<Result<Guid, Error>> AddAsync(FileData file, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<FileData>?> GetAsync(IEnumerable<Guid> fileIds, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> RemoveManyAsync(IEnumerable<Guid> fileIds, CancellationToken cancellationToken = default);
}
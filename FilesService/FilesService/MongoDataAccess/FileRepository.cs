using CSharpFunctionalExtensions;
using FilesService.Core;
using FilesService.Core.Models;
using MongoDB.Driver;

namespace FilesService.MongoDataAccess;

public class FileRepository : IFileRepository
{
    private readonly FileMongoDbContext _context;

    public FileRepository(FileMongoDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid, Error>> AddAsync(FileData file, CancellationToken cancellationToken = default)
    {
        await _context.Files.InsertOneAsync(file, cancellationToken: cancellationToken);
        
        return file.Id;
    }

    public async Task<IReadOnlyCollection<FileData>> GetAsync(IEnumerable<Guid> fileIds,
        CancellationToken cancellationToken = default)
    {
        return await _context.Files.Find(f => fileIds.Contains(f.Id)).ToListAsync(cancellationToken);
    }

    public async Task<UnitResult<Error>> RemoveManyAsync(IEnumerable<Guid> fileIds,
        CancellationToken cancellationToken = default)
    { 
        var deleteResult = await _context.Files.DeleteManyAsync(f => fileIds.Contains(f.Id), cancellationToken);

        if (deleteResult.DeletedCount == 0)
            return Errors.Files.FailRemove();

        return Result.Success<Error>();
    }
}
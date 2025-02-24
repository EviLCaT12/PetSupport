using Microsoft.Extensions.Logging;
using PetFamily.Application.Files;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using FileInfo = PetFamily.Application.Files.FileInfo;

namespace PetFamily.Infrastructure.Files;

public class FilesCleanerService : IFileCleanerService
{
    private readonly ILogger<FilesCleanerService> _logger;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly IFileProvider _fileProvider;

    public FilesCleanerService(        
        ILogger<FilesCleanerService> logger,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IFileProvider fileProvider)
    {
        _logger = logger;
        _messageQueue = messageQueue;
        _fileProvider = fileProvider;
    }

    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        var fileInfos = await _messageQueue.ReadAsync(cancellationToken);
        await _fileProvider.RemoveFilesAsync(fileInfos, cancellationToken);
    }
    
}
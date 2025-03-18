using Microsoft.Extensions.Logging;
using PetFamily.Core.Files;
using PetFamily.Core.Messaging;
using PetFamily.Core.Providers;
using FileInfo = PetFamily.Core.Files.FileInfo;

namespace PetFamily.Volunteer.Infrastructure.Files;

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
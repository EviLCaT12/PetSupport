using FilesService.Infrastructure;
using FilesService.MongoDataAccess;
using Hangfire;

namespace FilesService.Jobs;

public class ConfirmConsistencyJob(
    IFileRepository fileRepository, 
    IFileProvider fileProvider,
    string jobId,
    ILogger<ConfirmConsistencyJob> logger)
{
    [AutomaticRetry(Attempts = 3)]
    public async Task Execute(Guid fileId, string key)
    {
        logger.LogInformation($"Consistency job executed for {fileId}");

        var isFileInS3Task = fileProvider.GetPresignedUrlAsync(key);

        var isFileInMongoTask = fileRepository.GetAsync([fileId]);

        await Task.WhenAll(isFileInS3Task, isFileInMongoTask);

        var isFileInS3 = isFileInS3Task.Result; 
        
        var isFileInMongo = isFileInMongoTask.Result; 

        //Ситуация, когда есть в s3, но нет в монго
        if (isFileInS3 != null && (isFileInMongo == null || isFileInMongo.Count == 0))
        {
            logger.LogInformation($"File {fileId} hasnt already been upload on mongo");
            await fileProvider.DeletePresignedUrlAsync(key);
        }
        
        //Ситуация, когда есть в монго, но нет в s3
        if (isFileInS3 == null && (isFileInMongo != null || isFileInMongo.Count > 0))
        {
            logger.LogInformation($"File {fileId} hasnt already been upload on s3");
            await fileRepository.RemoveManyAsync([fileId]);
        }
        
    }
}
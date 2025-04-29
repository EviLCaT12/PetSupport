using Amazon.S3.Model;
using FilesService.Features;

namespace FilesService.Infrastructure;

public interface IFileProvider
{
    Task<string> UploadPresignedUrlAsync(string contentType, Guid key);
    
    Task<string> UploadPartPresignedUrlAsync(
        string uploadId,
        int partNumber,
        Guid key);
    
    Task<InitiateMultipartUploadResponse?> GetInitialMuplipartUploadPresignedUrlAsync(
        string contentType, 
        Guid key,
        CancellationToken cancellationToken);

    Task<CompleteMultipartUploadResponse?> CompleteMultipartUploadAsync(
        Guid key,
        string uploadId,
        List<CompleteMultipartUpload.PartETagInfo> parts,
        CancellationToken cancellationToken = default);
    
    Task<string> DeletePresignedUrlAsync(Guid key);
    
    Task<string> GetPresignedUrlAsync(Guid key);
}
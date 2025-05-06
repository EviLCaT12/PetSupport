using Amazon.S3.Model;
using FileService.Contracts;
using FilesService.Features;

namespace FilesService.Infrastructure;

public interface IFileProvider
{
    Task<string> UploadPresignedUrlAsync(string contentType, string key);
    
    Task<string> UploadPartPresignedUrlAsync(
        string uploadId,
        int partNumber,
        string key);
    
    Task<InitiateMultipartUploadResponse?> GetInitialMuplipartUploadPresignedUrlAsync(
        string contentType, 
        string key,
        CancellationToken cancellationToken);

    Task<CompleteMultipartUploadResponse?> CompleteMultipartUploadAsync(string key,
        string uploadId,
        List<PartETagInfo> parts,
        CancellationToken cancellationToken = default);
    
    Task<string> DeletePresignedUrlAsync(string key);
    
    Task<string?> GetPresignedUrlAsync(string storagePath);

    Task<GetObjectMetadataResponse?> GetObjectMetaDataAsync(string key, CancellationToken cancellationToken = default);
}
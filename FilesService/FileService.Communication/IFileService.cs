using CSharpFunctionalExtensions;
using FileService.Contracts.Requests;
using FileService.Contracts.Responses;

namespace FileService.Communication;

public interface IFileService
{
    Task<Result<IReadOnlyList<FileResponse>, string>> GetFilesPresignedUrls(
        GetFilesPresignedUrlRequest request, CancellationToken cancellationToken);

    Task<Result<string>> DeletePresignedUrl(
        DeletePresignedUrlRequest request, CancellationToken cancellationToken);

    Task<Result<UploadPresignedPartUrlResponse, string>> UploadPresignedUrl(
        UploadPresignedUrlRequest request, CancellationToken cancellationToken);

    Task<Result<StartMultipartUploadResponse, string>> StartMultipartUpload(
        StartMultipartUploadRequest request, CancellationToken cancellationToken);

    Task<Result<UploadPresignedPartUrlResponse, string>> UploadPresignedPartUrl(
        UploadPresignedPartUrlRequest request, CancellationToken cancellationToken);

    Task<Result<CompleteMultipartUploadResponse, string>> CompleteMultipartUpload(
        CompleteMultipartUploadRequest request, CancellationToken cancellationToken);
}
namespace FileService.Contracts.Requests;

public record UploadPresignedUrlRequest(string ContentType, long FileSize);
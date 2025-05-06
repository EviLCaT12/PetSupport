namespace FileService.Contracts;

public record UploadPresignedUrlRequest(string ContentType, long FileSize);
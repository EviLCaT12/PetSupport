namespace FileService.Contracts.Requests;

public record UploadPresignedPartUrlRequest(string Key ,string UploadId, int PartNumber);
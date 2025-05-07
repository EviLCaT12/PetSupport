namespace FileService.Contracts.Requests;

public record DeletePresignedUrlRequest(string Key ,IEnumerable<Guid> FileIds);
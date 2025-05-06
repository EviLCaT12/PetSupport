namespace FileService.Contracts.Requests;

public record GetFilesPresignedUrlRequest(IEnumerable<Guid> FileIds);
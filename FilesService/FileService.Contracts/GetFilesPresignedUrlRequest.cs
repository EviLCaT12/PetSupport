namespace FileService.Contracts;

public record GetFilesPresignedUrlRequest(IEnumerable<Guid> FileIds);
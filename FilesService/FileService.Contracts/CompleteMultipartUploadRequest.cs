namespace FileService.Contracts;
public record PartETagInfo(int PartNumber, string ETag);

public record CompleteMultipartUploadRequest(string UploadId, List<PartETagInfo> Parts);
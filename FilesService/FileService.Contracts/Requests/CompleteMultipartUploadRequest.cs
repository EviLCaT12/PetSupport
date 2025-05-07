namespace FileService.Contracts.Requests;
public record PartETagInfo(int PartNumber, string ETag);

public record CompleteMultipartUploadRequest(string Key, string UploadId, List<PartETagInfo> Parts);
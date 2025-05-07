using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using FileService.Contracts;
using FileService.Contracts.Requests;
using FileService.Contracts.Responses;

namespace FileService.Communication;

public class FileHttpClient(HttpClient httpClient) : IFileService
{
    public async Task<Result<IReadOnlyList<FileResponse>, string>> GetFilesPresignedUrls(
        GetFilesPresignedUrlRequest request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync("files/presigned-urls", request, cancellationToken);
        
        if (response.IsSuccessStatusCode == false)
            return "Fail to get files presigned urls";
        
        var fileResponses = await response.Content.ReadFromJsonAsync<IEnumerable<FileResponse>>(cancellationToken);
        
        return fileResponses?.ToList() ?? [];
    }

    public async Task<Result<string>> DeletePresignedUrl(
        DeletePresignedUrlRequest request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync("files/delete-presigned", request ,cancellationToken);
        
        if (response.IsSuccessStatusCode == false)
            return "Fail to delete presigned url";
        
        var fileResponse = await response.Content.ReadFromJsonAsync<string>(cancellationToken);
        
        return fileResponse;
    }

    public async Task<Result<UploadPresignedPartUrlResponse, string>> UploadPresignedUrl(
        UploadPresignedUrlRequest request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync("files/upload-presigned", request, cancellationToken);
        
        if (response.IsSuccessStatusCode == false)
            return "Fail to upload presigned url";
        
        var fileResponse = await response.Content.ReadFromJsonAsync<UploadPresignedPartUrlResponse>(cancellationToken);
        
        return fileResponse;
        
    }

    public async Task<Result<StartMultipartUploadResponse, string>> StartMultipartUpload(
        StartMultipartUploadRequest request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync("files/multipart", request, cancellationToken);
        
        if (response.IsSuccessStatusCode == false)
            return "Fail to start multipart upload";
        
        var fileResponse = await response.Content.ReadFromJsonAsync<StartMultipartUploadResponse>(cancellationToken);
        
        return fileResponse;
    }

    public async Task<Result<UploadPresignedPartUrlResponse, string>> UploadPresignedPartUrl(
        UploadPresignedPartUrlRequest request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync(
            "files/upload-presigned-part", 
            request,
            cancellationToken);
        
        if (response.IsSuccessStatusCode == false)
            return "Fail to upload presigned part url";
        
        var fileResponse = await response.Content.ReadFromJsonAsync<UploadPresignedPartUrlResponse>(cancellationToken);
        
        return fileResponse;
    }
    
    public async Task<Result<CompleteMultipartUploadResponse, string>> CompleteMultipartUpload(
        CompleteMultipartUploadRequest request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync(
            "files/complete-mupltipart", 
            request, 
            cancellationToken);
        
        if (response.IsSuccessStatusCode == false)
            return "Fail to complete multipart upload";
        
        var fileResponse = await response.Content.ReadFromJsonAsync<CompleteMultipartUploadResponse>(cancellationToken);
        
        return fileResponse;
    }
    
    
}
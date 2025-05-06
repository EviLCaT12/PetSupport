namespace FileService.Communication;

public class FileHttpClient(HttpClient httpClient)
{
    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        return await httpClient.GetAsync(url);
    }
}
namespace FilesService.Error.Models;

public static class Errors
{
    public static class FileProviderErrors
    {
        public static Error EmptyPresignedUrl()
        {
            return Error.Failure("presigned.url.error", "Provider returned an empty URL.");
        }
        
    }
}
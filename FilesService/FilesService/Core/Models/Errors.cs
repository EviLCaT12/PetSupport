namespace FilesService.Core.Models;

public static class Errors
{
    public static class FileProviderErrors
    {
        public static Error EmptyPresignedUrl()
        {
            return Error.Failure("presigned.url.error", "Provider returned an empty URL.");
        }
        
    }

    public static class Files
    {
        public static Error FailRemove() =>
            Error.Failure("file.remove", "Fail to remove file.");
    }
}
using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.SharedVO;

public record FilePath
{
    [JsonConstructor]
    private FilePath(string path)
    {
        Path = path;
    }
    
    public string Path { get; }
    

    public static Result<FilePath, Error.Error> Create(string path, string? extension) 
        //Расширение опционально из-за возможности фронта кидать уже полное имя файла.
    {
        if (string.IsNullOrEmpty(extension))
        {
            return new FilePath(path);
        }
        
        return new FilePath(path + extension);
    }
}
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.Shared.SharedVO;

public record FilePath
{
    private FilePath(string path)
    {
        Path = path;
    }
    
    public string Path { get; }
    

    public static Result<FilePath, ErrorList> Create(Guid path, string? extension) 
        //Расширение опционально из-за возможности фронта кидать уже полное имя файла.
    {
        var fullPath = path.ToString();
        if (string.IsNullOrEmpty(extension))
        {
            return new FilePath(fullPath);
        }
        
        return new FilePath(fullPath + extension);
    }
}
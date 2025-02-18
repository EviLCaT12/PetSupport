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
    

    public static Result<FilePath, ErrorList> Create(string path, string? extension) 
        //Расширение опционально из-за возможности фронта кидать уже полное имя файла.
    {
        if (string.IsNullOrEmpty(extension))
        {
            return new FilePath(path);
        }
        
        return new FilePath(path + extension);
    }
}
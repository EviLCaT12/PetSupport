using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record FilePath
{
    private FilePath(string path)
    {
        Path = path;
    }
    
    public string Path { get; }
    

    public static Result<FilePath, ErrorList> Create(Guid path, string extension)
    {
        var fullPath = path.ToString() + "." + extension;
        return new FilePath(fullPath);
    }
}
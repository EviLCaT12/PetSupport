using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record PetPhoto
{
    //ef core
    public PetPhoto() { }
    
    [JsonConstructor]
    private PetPhoto(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }
    public FilePath PathToStorage { get; }

    public bool IsMain { get; private set; } = false;
    
    public static int CountMainPhoto = 0;

    public static Result<PetPhoto, ErrorList> Create(FilePath photoFilePath)
    {
        return new PetPhoto(photoFilePath);
    }

    public UnitResult<ErrorList> SetMain()
    {
        if (IsMain)
        {
            var error = Error.Failure("invalid.pet.operation",
                $"This photo {PathToStorage.Path} is already set to main.");
            return new ErrorList([error]);
        }
        
        IsMain = true;
        return new UnitResult<ErrorList>();
    }
    
    
    public void RemoveMain()
        => IsMain = false;
}

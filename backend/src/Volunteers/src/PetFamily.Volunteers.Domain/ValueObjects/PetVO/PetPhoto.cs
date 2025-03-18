using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Volunteers.Domain.ValueObjects.PetVO;

public record PetPhoto
{
    //ef core
    public PetPhoto() { }
    private PetPhoto(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }
    
    [JsonConstructor]
    private PetPhoto(FilePath pathToStorage, bool isMain)
    {
        PathToStorage = pathToStorage;
        IsMain = isMain;
    }
    public FilePath PathToStorage { get; }

    public bool IsMain { get; private set; }
    
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
        return Result.Success<ErrorList>();
    }
    
    
    public UnitResult<ErrorList> RemoveMain()
    {
        if (IsMain == false)
        {
            var error = Error.Failure("invalid.pet.operation",
                $"This photo {PathToStorage.Path} is already not a main photo.");
            return new ErrorList([error]);
        }
        
        IsMain = false;
        return Result.Success<ErrorList>();
    }
}

using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.SharedKernel.SharedVO;

public class Photo : ValueObject
{
    //ef core
    public Photo() { }
    private Photo(FileId fileId) => Id = fileId;
    
    [JsonConstructor]
    private Photo(FileId fileId, bool isMain)
    {
        Id = fileId;
        IsMain = isMain;
    }

    public FileId Id { get; init; }
    
    public static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png"};
    
    public bool IsMain { get; private set; }
    
    public static int CountMainPhoto = 0;

    public static Result<Photo, ErrorList> Create(FileId fileId, string fileType)
    {
        if (AllowedExtensions.Contains(fileType) == false)
            return Errors.General.ValueIsInvalid(nameof(fileType)).ToErrorList();
        
        return new Photo(fileId);
    }

    public UnitResult<ErrorList> SetMain()
    {
        if (IsMain)
        {
            var error = Error.Error.Failure("invalid.pet.operation",
                $"This photo {Id} is already set to main.");
            return new ErrorList([error]);
        }
        
        IsMain = true;
        return Result.Success<ErrorList>();
    }
    
    
    public UnitResult<ErrorList> RemoveMain()
    {
        if (IsMain == false)
        {
            var error = Error.Error.Failure("invalid.pet.operation",
                $"This photo {Id} is already not a main photo.");
            return new ErrorList([error]);
        }
        
        IsMain = false;
        return Result.Success<ErrorList>();
    }

    public Photo Copy()
    {
        return new Photo(Id);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Id;
    }
}

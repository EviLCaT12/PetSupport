using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Constants;

namespace PetFamily.Domain.Shared.SharedVO;

public record Description 
{
    public string Value { get; }
    
    private Description(string value) => Value = value;
    
    public static Result<Description> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Description>("Description cannot be empty");
        
        if (description.Length > VolunteerConstant.MAX_DESCRIPTION_LENGHT)
            return Result.Failure<Description>("Description cannot be longer than " + VolunteerConstant.MAX_DESCRIPTION_LENGHT + " symbols");

        var validDescription = new Description(description);
        
        return Result.Success(validDescription);
    }
}
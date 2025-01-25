using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.Shared.SharedVO;

public record Description 
{
    public string Value { get; }
    
    private Description(string value) => Value = value;
    
    public static Result<Description, Error.Error> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return ErrorList.General.ValueIsRequired(nameof(Description));
        
        if (description.Length > VolunteerConstant.MAX_DESCRIPTION_LENGHT)
            return ErrorList.General.LengthIsInvalid(VolunteerConstant.MAX_DESCRIPTION_LENGHT,nameof(Description));

        var validDescription = new Description(description);
        
        return validDescription;
    }
}
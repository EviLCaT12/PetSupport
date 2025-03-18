using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Constants;
using PetFamily.SharedKernel.Error;

namespace PetFamily.SharedKernel.SharedVO;

public record Description 
{
    public string Value { get; }
    
    private Description(string value) => Value = value;
    
    public static Result<Description, Error.Error> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsRequired(nameof(Description));
        
        if (description.Length > VolunteerConstant.MAX_DESCRIPTION_LENGHT)
            return Errors.General.LengthIsInvalid(VolunteerConstant.MAX_DESCRIPTION_LENGHT,nameof(Description));

        var validDescription = new Description(description);
        
        return validDescription;
    }
}
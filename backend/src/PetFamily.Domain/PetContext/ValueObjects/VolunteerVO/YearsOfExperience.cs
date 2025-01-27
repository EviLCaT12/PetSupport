using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

public record YearsOfExperience
{
    private YearsOfExperience(int value)
    {
        Value = value;
    }
    
    public int Value { get;}

    public static Result<YearsOfExperience, Error> Create(int years)
    {
        if (years < 0)
            return ErrorList.General.ValueIsInvalid(nameof(YearsOfExperience));
        
        var validYears = new YearsOfExperience(years);
        
        return validYears;
    }
}
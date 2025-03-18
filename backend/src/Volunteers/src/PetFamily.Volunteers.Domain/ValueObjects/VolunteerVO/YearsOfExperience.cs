using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

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
            return Errors.General.ValueIsInvalid(nameof(YearsOfExperience));
        
        var validYears = new YearsOfExperience(years);
        
        return validYears;
    }
}
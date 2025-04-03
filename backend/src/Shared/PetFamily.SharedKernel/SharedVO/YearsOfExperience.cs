using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.SharedKernel.SharedVO;

public record YearsOfExperience
{
    private YearsOfExperience() { }
    
    private YearsOfExperience(int value)
    {
        Value = value;
    }
    
    public int Value { get;}

    public static Result<YearsOfExperience, Error.Error> Create(int years)
    {
        if (years < 0)
            return Errors.General.ValueIsInvalid(nameof(YearsOfExperience));
        
        var validYears = new YearsOfExperience(years);
        
        return validYears;
    }
}
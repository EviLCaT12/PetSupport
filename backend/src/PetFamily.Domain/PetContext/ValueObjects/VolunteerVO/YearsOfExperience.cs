using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

public record YearsOfExperience
{
    private YearsOfExperience(int years)
    {
        Value = years;
    }
    
    public int Value { get;}

    public static Result<YearsOfExperience> Create(int years)
    {
        if (years < 0)
            return Result.Failure<YearsOfExperience>("Years cannot be negative");
        
        var validYears = new YearsOfExperience(years);
        
        return Result.Success(validYears);
    }
}
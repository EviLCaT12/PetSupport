using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Constants;

namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record HealthInfo 
{
    private HealthInfo(string value)
    {
        Value = value;
    }
    
    public string Value { get;}

    public static Result<HealthInfo> Create(string healthInfo)
    {
        if (string.IsNullOrWhiteSpace(healthInfo))
            return Result.Failure<HealthInfo>("HealthInfo cannot be empty");
        
        if (healthInfo.Length > VolunteerConstant.MAX_DESCRIPTION_LENGHT)
            return Result.Failure<HealthInfo>("HealthInfo cannot be longer than " + VolunteerConstant.MAX_DESCRIPTION_LENGHT + " symbols");

        var validDescription = new HealthInfo(healthInfo);
        
        return Result.Success(validDescription);
    }
}
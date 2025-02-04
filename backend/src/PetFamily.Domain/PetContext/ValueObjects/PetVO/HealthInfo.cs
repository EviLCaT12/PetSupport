using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record HealthInfo 
{
    private HealthInfo(string value)
    {
        Value = value;
    }
    
    public string Value { get;}

    public static Result<HealthInfo, Error> Create(string healthInfo)
    {
        if (string.IsNullOrWhiteSpace(healthInfo))
            return Errors.General.ValueIsRequired(nameof(HealthInfo));
        
        if (healthInfo.Length > VolunteerConstant.MAX_DESCRIPTION_LENGHT)
            return Errors.General.LengthIsInvalid(VolunteerConstant.MAX_DESCRIPTION_LENGHT, nameof(HealthInfo));

        var validHealthInfo = new HealthInfo(healthInfo);

        return validHealthInfo;
    }
}
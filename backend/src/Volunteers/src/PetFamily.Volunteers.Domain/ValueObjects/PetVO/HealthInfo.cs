using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Constants;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Volunteers.Domain.ValueObjects.PetVO;

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
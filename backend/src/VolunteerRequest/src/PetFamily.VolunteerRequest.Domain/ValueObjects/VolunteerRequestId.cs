using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.VolunteerRequest.Domain.ValueObjects;

public class VolunteerRequestId : ValueObject
{
    // ef core
    private VolunteerRequestId() { }
    
    private VolunteerRequestId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static Result<VolunteerRequestId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Errors.General.ValueIsRequired(nameof(value));

        return new VolunteerRequestId(value);
    }
    
    public static VolunteerRequestId NewVolunteerRequestId() => new VolunteerRequestId(Guid.NewGuid());
    
    public static VolunteerRequestId EmptyVolunteerRequestId() => new VolunteerRequestId(Guid.Empty);
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
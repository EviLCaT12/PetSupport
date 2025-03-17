using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

public record VolunteerId : IComparable<VolunteerId>
{
    public Guid Value { get;}

    private VolunteerId(Guid value)
    {
        Value = value;
    }
    
    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());
    public static VolunteerId EmptyVolunteerId() => new(Guid.Empty);

    public static Result<VolunteerId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
            return Errors.General.ValueIsRequired(nameof(VolunteerId));

        return new VolunteerId(id);
    }
    
    public int CompareTo(VolunteerId? other)
    {
        if(other == null)
            return 1;
        
        return Value.CompareTo(other.Value);
    }
}
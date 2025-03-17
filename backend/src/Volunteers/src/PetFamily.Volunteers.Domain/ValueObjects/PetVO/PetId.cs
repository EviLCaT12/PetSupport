using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Domain.ValueObjects.PetVO;

public record PetId : IComparable<PetId>
{
    public Guid Value { get; }
    
    private PetId(Guid value) => Value = value;
    
    public static PetId NewPetId() => new(Guid.NewGuid());
    public static PetId EmptyPetId() => new(Guid.Empty);
    public static Result<PetId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
            return Errors.General.ValueIsRequired(nameof(VolunteerId));

        return new PetId(id);
    }
    public int CompareTo(PetId? other)
    {
        if(other == null)
            return 1;
        
        return Value.CompareTo(other.Value);
    }
}

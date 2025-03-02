using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

public record SpeciesId : IComparable<SpeciesId>
{
    // ef core
    private SpeciesId() { }

    private SpeciesId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static Result<SpeciesId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            var error = Errors.General.ValueIsRequired(nameof(SpeciesId));
            return error;
        }
        
        return new SpeciesId(value);
    }
    public static SpeciesId NewSpeciesId() => new(Guid.NewGuid());
    public static SpeciesId EmptySpeciesId() => new(Guid.Empty);


    public int CompareTo(SpeciesId? other)
    {
        if(other == null)
            return 1;
        
        return Value.CompareTo(other.Value);
    }
}
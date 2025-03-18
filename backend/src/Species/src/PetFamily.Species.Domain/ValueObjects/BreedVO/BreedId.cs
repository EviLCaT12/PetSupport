using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.Species.Domain.ValueObjects.SpeciesVO;

namespace PetFamily.Species.Domain.ValueObjects.BreedVO;

public record BreedId : IComparable<BreedId>
{
    // ef core
    private BreedId() { }

    private BreedId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static Result<BreedId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            var error = Errors.General.ValueIsRequired(nameof(SpeciesId));
            return error;
        }
        
        return new BreedId(value);
    }
    public static BreedId NewBreedId() => new(Guid.NewGuid());
    public static BreedId EmptyBreedId() => new(Guid.Empty);
    public int CompareTo(BreedId? other)
    {
        if(other == null)
            return 1;
        
        return Value.CompareTo(other.Value);
    }
}

namespace PetFamily.Domain.SpeciesContext.ValueObjects.BreedVO;

public record BreedId : IComparable<BreedId>
{
    // ef core
    private BreedId() { }

    private BreedId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static BreedId Create(Guid value) => new(value);
    public static BreedId NewBreedId() => new(Guid.NewGuid());
    public static BreedId EmptyBreedId() => new(Guid.Empty);
    public int CompareTo(BreedId? other)
    {
        if(other == null)
            return 1;
        
        return Value.CompareTo(other.Value);
    }
}

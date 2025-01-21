namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record PetId : IComparable<PetId>
{
    public Guid Value { get; }
    
    private PetId(Guid value) => Value = value;
    
    public static PetId NewPetId() => new(Guid.NewGuid());
    public static PetId EmptyPetId() => new(Guid.Empty);
    public static PetId Create(Guid id) => new PetId(id);
    public int CompareTo(PetId? other)
    {
        if(other == null)
            return 1;
        
        return Value.CompareTo(other.Value);
    }
}

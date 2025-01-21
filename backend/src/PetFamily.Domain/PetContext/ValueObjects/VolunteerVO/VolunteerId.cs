namespace PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

public record VolunteerId : IComparable<VolunteerId>
{
    public Guid Value { get;}

    private VolunteerId(Guid value)
    {
        Value = value;
    }
    
    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());
    public static VolunteerId EmptyVolunteerId() => new(Guid.Empty);
    public static VolunteerId Create(Guid id) => new(id);
    
    public int CompareTo(VolunteerId? other)
    {
        if(other == null)
            return 1;
        
        return Value.CompareTo(other.Value);
    }
}
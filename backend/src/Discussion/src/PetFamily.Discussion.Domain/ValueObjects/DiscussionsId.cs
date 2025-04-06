using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Discussion.Domain.ValueObjects;

public class DiscussionsId : ValueObject
{
    private DiscussionsId() { }

    private DiscussionsId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static Result<DiscussionsId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Errors.General.ValueIsRequired(nameof(value));

        return new DiscussionsId(value);
    }

    public static DiscussionsId Empty() => new (Guid.Empty);

    public static DiscussionsId NewId() => new (Guid.NewGuid());
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
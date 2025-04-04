using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Discussion.Domain.ValueObjects;

public class MessageId : ValueObject
{
    private MessageId() { }

    private MessageId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static Result<MessageId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Errors.General.ValueIsRequired(nameof(value));

        return new MessageId(value);
    }

    public static MessageId Empty() => new (Guid.Empty);

    public static MessageId NewId() => new (Guid.NewGuid());
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
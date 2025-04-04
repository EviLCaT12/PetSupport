using CSharpFunctionalExtensions;
using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Domain.Entities;

public class Message : Entity<MessageId>
{
    internal MessageId Id { get; private set; }

    internal Guid UserId { get; private set; }

    internal Text Text { get; private set; }

    internal DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    internal bool IsEdited { get; private set; } = false;

    internal void Edit(Text text)
    {
        Text = text;
        IsEdited = true;
    }

}
using CSharpFunctionalExtensions;
using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Domain.Entities;

public class Message : Entity<MessageId>
{
    private Message() { }
    public Message(
        MessageId id,
        Guid userId,
        Text text)
    {
        Id = id;
        UserId = userId;
        Text = text;
    }
    
    public MessageId Id { get; private set; }

    public Discussion Discussion { get; private set; } = null!;

    public Guid UserId { get; private set; }

    public Text Text { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public bool IsEdited { get; private set; } = false;

    internal void Edit(Text text)
    {
        Text = text;
        IsEdited = true;
    }

}
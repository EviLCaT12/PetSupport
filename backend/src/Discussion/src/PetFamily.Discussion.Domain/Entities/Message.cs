using CSharpFunctionalExtensions;
using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Domain.Entities;

public class Message : Entity<MessageId> 
{
    public MessageId Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public Text Text { get; private set; }
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    public bool IsEdited { get; private set; } = false;
    
    
}
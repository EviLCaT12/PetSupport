namespace Contracts.Dtos;

public class MessageDto
{
    public Guid Id { get; init; }
    
    public Guid DiscussionId { get; init; }
    
    public Guid UserId { get; init; }
    
    public string Text { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public bool isEdited { get; init; }
}
namespace Contracts.Dtos;

public class DiscussionDto
{
    public Guid Id { get; init; }
    
    public Guid RelationId { get; init; }
    
    public Guid[] Members { get; init; }
    
    public IReadOnlyList<MessageDto> Messages{ get; init; }
    
    public string Status { get; init; }
    
}
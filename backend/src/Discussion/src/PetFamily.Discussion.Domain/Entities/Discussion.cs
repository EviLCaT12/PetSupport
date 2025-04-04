using CSharpFunctionalExtensions;
using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Domain.Entities;

public class Discussion : Entity<DiscussionsId>
{
    public DiscussionsId Id { get; private set; }
    
    public Guid RelationId { get; private set; }

    private List<Guid> _users = [];
    
    public IReadOnlyList<Guid> Users => _users;
    
    private List<Message> _messages = [];
    
    public IReadOnlyList<Message> Messages => _messages;
    
}
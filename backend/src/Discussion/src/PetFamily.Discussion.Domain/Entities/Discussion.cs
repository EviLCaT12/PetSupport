using CSharpFunctionalExtensions;
using PetFamily.Discussion.Domain.Enums;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Discussion.Domain.Entities;

public class Discussion : Entity<DiscussionsId>
{
    private Discussion() { }

    private Discussion(
        DiscussionsId id,
        Guid relationId,
        IEnumerable<Guid> users)
    {
        Id = id;
        RelationId = relationId;
        _members = users.ToList();
    }
    
    public DiscussionsId Id { get; private set; }
    
    public Guid RelationId { get; private set; }

    private List<Guid> _members = [];
    
    public IReadOnlyList<Guid> Members => _members;
    
    private List<Message> _messages = [];
    
    public IReadOnlyList<Message> Messages => _messages;

    public Status Status { get; private set; } = Status.Open;

    public static Result<Discussion, Error> Create(
        DiscussionsId id,
        Guid relationId,
        IEnumerable<Guid>? users)
    {
        if (relationId == Guid.Empty)
            return Errors.General.ValueIsRequired(nameof(relationId));
        
        
        if (users is null || users.Count() < 2)
            return Errors.General.ValueIsInvalid(nameof(users));
        
        return new Discussion(id, relationId, users);
    }

    public Result<bool, Error> IsUserInDiscussion(Guid userId)
    {
        var isUserInDiscussion = _members.Contains(userId);
        if (isUserInDiscussion == false)
            return Errors.General.ValueIsInvalid("User does not belong to this discussion");

        return isUserInDiscussion;
    }

    public void AddComment(Message comment)
    {
        _messages.Add(comment);
    }

    public UnitResult<Error> DeleteComment(Message comment)
    {
        _messages.Remove(comment);
        
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> EditComment(Message comment, Text newText)
    {
        comment.Edit(newText);
        
        return UnitResult.Success<Error>();
    }
    
    public Result<bool, Error> IsMessageInDiscussion(Message message)
    {
        var isMessageInDiscussion = _messages.Contains(message);
        if (isMessageInDiscussion == false)
            return Errors.General.ValueIsInvalid("Message does not belong to this discussion");

        return isMessageInDiscussion;
    }
    
    public Result<bool, Error> IsCommentBelongToUser(Message comment, Guid userId)
    {
        var result = comment.UserId == userId;
        if (result == false)
            return Errors.General.ValueIsInvalid("Comment does not belong to this user");

        return result;
    }

    public void Close()
    {
        Status = Status.Closed;
    }
}
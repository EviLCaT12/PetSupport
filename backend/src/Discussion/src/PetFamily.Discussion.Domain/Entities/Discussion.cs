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
        _users = users.ToList();
    }
    
    public DiscussionsId Id { get; private set; }
    
    public Guid RelationId { get; private set; }

    private List<Guid> _users = [];
    
    public IReadOnlyList<Guid> Users => _users;
    
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
        var isUserInDiscussion = _users.Contains(userId);
        if (isUserInDiscussion == false)
            return Errors.General.ValueIsInvalid("User does not belong to this discussion");

        return isUserInDiscussion;
    }

    public UnitResult<Error> AddComment(Message comment, Guid userId)
    {
        var isUserInDiscussion = IsUserInDiscussion(userId);
        if (isUserInDiscussion.IsFailure)
            return isUserInDiscussion.Error;
        
        _messages.Add(comment);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> DeleteComment(Message comment, Guid userId)
    {
        var isUserInDiscussion = IsUserInDiscussion(userId);
        if (isUserInDiscussion.IsFailure)
            return isUserInDiscussion.Error;
        
        var isCommentBelongToUser = IsCommentBelongToUser(comment, userId);
        if (isCommentBelongToUser.IsFailure)
            return isCommentBelongToUser.Error;
        
        _messages.Remove(comment);
        
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> EditComment(Message comment, Text newText, Guid userId)
    {
        var isMessageInDiscussion = IsMessageInDiscussion(comment);
        if (isMessageInDiscussion.IsFailure)
            return isMessageInDiscussion.Error;
        
        var isCommentBelongToUser = IsCommentBelongToUser(comment, userId);
        if (isCommentBelongToUser.IsFailure)
            return isCommentBelongToUser.Error;
        
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
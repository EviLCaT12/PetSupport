using FluentAssertions;
using PetFamily.Discussion.Domain.Entities;
using PetFamily.Discussion.Domain.Enums;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.SharedKernel.Error;

namespace UnitTests;

public class DiscussionTests
{
    private readonly DiscussionsId _validId = DiscussionsId.NewId();
    private readonly Guid _validRelationId = Guid.NewGuid();
    private readonly List<Guid> _validUsers = new() { Guid.NewGuid(), Guid.NewGuid() };
    
    [Fact]
    public void Create_WithValidParameters_ReturnsDiscussion()
    {
        // Act
        var result = Discussion.Create(_validId, _validRelationId, _validUsers);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(_validId);
        result.Value.RelationId.Should().Be(_validRelationId);
        result.Value.Members.Should().BeEquivalentTo(_validUsers);
        result.Value.Status.Should().Be(Status.Open);
    }

    [Fact]
    public void Create_WithEmptyRelationId_ReturnsError()
    {
        // Arrange
        var emptyRelationId = Guid.Empty;

        // Act
        var result = Discussion.Create(_validId, emptyRelationId, _validUsers);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Create_WithNullUsers_ReturnsError()
    {
        // Act
        var result = Discussion.Create(_validId, _validRelationId, null);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Create_WithLessThanTwoUsers_ReturnsError()
    {
        // Arrange
        var singleUser = new List<Guid> { Guid.NewGuid() };

        // Act
        var result = Discussion.Create(_validId, _validRelationId, singleUser);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void IsUserInDiscussion_WithExistingUser_ReturnsTrue()
    {
        // Arrange
        var discussion = Discussion.Create(_validId, _validRelationId, _validUsers).Value;
        var existingUserId = _validUsers.First();

        // Act
        var result = discussion.IsUserInDiscussion(existingUserId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
    }

    [Fact]
    public void IsUserInDiscussion_WithNonExistingUser_ReturnsError()
    {
        // Arrange
        var discussion = Discussion.Create(_validId, _validRelationId, _validUsers).Value;
        var nonExistingUserId = Guid.NewGuid();

        // Act
        var result = discussion.IsUserInDiscussion(nonExistingUserId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.General.ValueIsInvalid("User does not belong to this discussion"));
    }

    [Fact]
    public void AddComment_WithValidUser_AddsComment()
    {
        // Arrange
        var discussion = Discussion.Create(_validId, _validRelationId, _validUsers).Value;
        var userId = _validUsers.First();
        var comment = new Message(MessageId.NewId(), userId, Text.Create("Test message").Value);

        // Act
        discussion.AddComment(comment);

        // Assert
        discussion.Messages.Should().Contain(comment);
    }
    

    [Fact]
    public void DeleteComment_WithValidUserAndComment_RemovesComment()
    {
        // Arrange
        var discussion = Discussion.Create(_validId, _validRelationId, _validUsers).Value;
        var userId = _validUsers.First();
        var comment = new Message(MessageId.NewId(), userId, Text.Create("Test message").Value);
        discussion.AddComment(comment);

        // Act
        var result = discussion.DeleteComment(comment);

        // Assert
        result.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().NotContain(comment);
    }

    [Fact]
    public void EditComment_WithValidUserAndComment_EditsComment()
    {
        // Arrange
        var discussion = Discussion.Create(_validId, _validRelationId, _validUsers).Value;
        var userId = _validUsers.First();
        var originalText = Text.Create("Original text").Value;
        var comment = new Message(MessageId.NewId(), userId, originalText);
        discussion.AddComment(comment);
        
        var newText = Text.Create("New text").Value;

        // Act
        var result = discussion.EditComment(comment, newText);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    

    [Fact]
    public void IsMessageInDiscussion_WithExistingMessage_ReturnsTrue()
    {
        // Arrange
        var discussion = Discussion.Create(_validId, _validRelationId, _validUsers).Value;
        var userId = _validUsers.First();
        var comment = new Message(MessageId.NewId(), userId, Text.Create("Test message").Value);
        discussion.AddComment(comment);

        // Act
        var result = discussion.IsMessageInDiscussion(comment);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
    }

    [Fact]
    public void IsMessageInDiscussion_WithNonExistingMessage_ReturnsError()
    {
        // Arrange
        var discussion = Discussion.Create(_validId, _validRelationId, _validUsers).Value;
        var userId = _validUsers.First();
        var comment = new Message(MessageId.NewId(), userId, Text.Create("Test message").Value);

        // Act
        var result = discussion.IsMessageInDiscussion(comment);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.General.ValueIsInvalid("Message does not belong to this discussion"));
    }

    [Fact]
    public void IsCommentBelongToUser_WithOwner_ReturnsTrue()
    {
        // Arrange
        var userId = _validUsers.First();
        var comment = new Message(MessageId.NewId(), userId, Text.Create("Test message").Value);

        // Act
        var result = Discussion.Create(_validId, _validRelationId, _validUsers).Value
            .IsCommentBelongToUser(comment, userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
    }

    [Fact]
    public void IsCommentBelongToUser_WithNonOwner_ReturnsError()
    {
        // Arrange
        var userId = _validUsers.First();
        var nonOwnerId = _validUsers.Last();
        var comment = new Message(MessageId.NewId(), userId, Text.Create("Test message").Value);
        // Act
        var result = Discussion.Create(_validId, _validRelationId, _validUsers).Value
            .IsCommentBelongToUser(comment, nonOwnerId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.General.ValueIsInvalid("Comment does not belong to this user"));
    }

    [Fact]
    public void Close_SetsStatusToClosed()
    {
        // Arrange
        var discussion = Discussion.Create(_validId, _validRelationId, _validUsers).Value;

        // Act
        discussion.Close();

        // Assert
        discussion.Status.Should().Be(Status.Closed);
    }
}
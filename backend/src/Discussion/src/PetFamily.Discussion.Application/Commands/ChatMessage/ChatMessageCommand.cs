using PetFamily.Core.Abstractions;

namespace PetFamily.Discussion.Application.Commands.ChatMessage;

public record ChatMessageCommand(
    Guid UserId,
    Guid DiscussionId,
    string Text) : ICommand;
using PetFamily.Core.Abstractions;

namespace PetFamily.Discussion.Application.Commands.DeleteMessage;

public record DeleteMessageCommand(
    Guid UserId,
    Guid DiscussionId,
    Guid MessageId) : ICommand;
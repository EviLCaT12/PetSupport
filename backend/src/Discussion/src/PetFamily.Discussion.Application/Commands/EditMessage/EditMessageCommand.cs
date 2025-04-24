using PetFamily.Core.Abstractions;

namespace PetFamily.Discussion.Application.Commands.EditMessage;

public record EditMessageCommand(
    Guid MessageId,
    Guid DiscussionId, 
    Guid UserId,
    string NewText) : ICommand;
using PetFamily.Core.Abstractions;

namespace PetFamily.Discussion.Application.Commands.Close;

public record CloseCommand(Guid DiscussionId, Guid UserId) : ICommand;
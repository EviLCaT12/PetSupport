using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Application.Commands.ChatMessage;

public class ChatMessageCommandValidator : AbstractValidator<ChatMessageCommand>
{
    public ChatMessageCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.DiscussionId).MustBeValueObject(DiscussionsId.Create);
        RuleFor(c => c.Text).MustBeValueObject(Text.Create);
    }
}
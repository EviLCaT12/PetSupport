using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Application.Commands.DeleteMessage;

public class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
{
    public DeleteMessageCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.MessageId).MustBeValueObject(MessageId.Create);
        RuleFor(c => c.DiscussionId).MustBeValueObject(DiscussionsId.Create);
    }
}
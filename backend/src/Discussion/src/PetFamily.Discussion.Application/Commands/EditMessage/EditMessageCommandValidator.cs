using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Application.Commands.EditMessage;

public class EditMessageCommandValidator : AbstractValidator<EditMessageCommand>
{
    public EditMessageCommandValidator()
    {
        RuleFor(c => c.MessageId).MustBeValueObject(MessageId.Create);
        RuleFor(c => c.DiscussionId).MustBeValueObject(DiscussionsId.Create);
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.NewText).MustBeValueObject(Text.Create);
    }
}
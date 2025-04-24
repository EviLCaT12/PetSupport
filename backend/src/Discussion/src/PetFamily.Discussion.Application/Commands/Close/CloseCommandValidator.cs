using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Application.Commands.Close;

public class CloseCommandValidator : AbstractValidator<CloseCommand>
{
    public CloseCommandValidator()
    {
        RuleFor(c => c.UserId).MustBeValueObject(DiscussionsId.Create);
        RuleFor(c => c.UserId).NotEmpty();
    }
}
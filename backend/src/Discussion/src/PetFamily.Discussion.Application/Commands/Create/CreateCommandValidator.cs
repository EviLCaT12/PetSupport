using FluentValidation;

namespace PetFamily.Discussion.Application.Commands.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(c => c.RelationId).NotEqual(Guid.Empty);
        RuleFor(c => c.Members).Must(x => x.Count() == 2);
        RuleForEach(c => c.Members).NotEqual(Guid.Empty);
    }
}
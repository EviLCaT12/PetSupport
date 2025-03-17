using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Species.Application.Commands.Create;

namespace PetFamily.Species.Application.Validators;

public class NameValidator : AbstractValidator<CreateCommand>
{
    public NameValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}
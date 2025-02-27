using FluentValidation;
using PetFamily.Application.SpeciesManagement.Create;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Validators.Species;

public class NameValidator : AbstractValidator<CreateCommand>
{
    public NameValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}
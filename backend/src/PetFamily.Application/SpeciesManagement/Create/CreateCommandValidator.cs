using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.SpeciesManagement.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}
using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.Species.Application.Commands.Remove;
using PetFamily.Species.Domain.ValueObjects.SpeciesVO;

namespace PetFamily.Species.Application.Validators;

public class RemoveSpeciesCommandValidator : AbstractValidator<RemoveSpeciesCommand>
{
    public RemoveSpeciesCommandValidator()
    {
        RuleFor(c => c.SpeciesId)
            .MustBeValueObject(SpeciesId.Create);
    }
}
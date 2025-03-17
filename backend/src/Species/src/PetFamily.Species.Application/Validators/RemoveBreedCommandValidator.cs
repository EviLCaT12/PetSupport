using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.Species.Application.Commands.RemoveBreed;
using PetFamily.Species.Domain.ValueObjects.BreedVO;
using PetFamily.Species.Domain.ValueObjects.SpeciesVO;

namespace PetFamily.Species.Application.Validators;

public class RemoveBreedCommandValidator : AbstractValidator<RemoveBreedCommand>
{
    public RemoveBreedCommandValidator()
    {
        RuleFor(c => c.SpeciesId)
            .MustBeValueObject(SpeciesId.Create);
        RuleFor(c => c.BreedId)
            .MustBeValueObject(BreedId.Create);
    }
}
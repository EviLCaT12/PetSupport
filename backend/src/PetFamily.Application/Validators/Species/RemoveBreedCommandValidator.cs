using FluentValidation;
using PetFamily.Application.SpeciesManagement.RemoveBreed;
using PetFamily.Domain.SpeciesContext.ValueObjects.BreedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Application.Validators.Species;

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
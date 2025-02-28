using FluentValidation;
using PetFamily.Application.SpeciesManagement.Commands.Remove;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Application.Validators.Species;

public class RemoveSpeciesCommandValidator : AbstractValidator<RemoveSpeciesCommand>
{
    public RemoveSpeciesCommandValidator()
    {
        RuleFor(c => c.SpeciesId)
            .MustBeValueObject(SpeciesId.Create);
    }
}
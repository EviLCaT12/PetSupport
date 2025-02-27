using FluentValidation;
using PetFamily.Application.SpeciesManagement.AddBreeds;
using PetFamily.Domain.Shared.SharedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Application.Validators.Species;

public class AddBreedsCommandValidator : AbstractValidator<AddBreedsCommand>
{
    public AddBreedsCommandValidator()
    {
        RuleFor(c => c.SpeciesId)
            .MustBeValueObject(SpeciesId.Create);
        RuleForEach(c => c.Names)
            .MustBeValueObject(Name.Create);
    }
}
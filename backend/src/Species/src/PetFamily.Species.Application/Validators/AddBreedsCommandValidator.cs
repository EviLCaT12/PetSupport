using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Species.Application.Commands.AddBreeds;
using PetFamily.Species.Domain.ValueObjects.SpeciesVO;

namespace PetFamily.Species.Application.Validators;

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
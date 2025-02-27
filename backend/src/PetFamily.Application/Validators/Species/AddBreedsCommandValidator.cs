using FluentValidation;
using PetFamily.Application.SpeciesManagement.AddBreeds;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Validators.Species;

public class AddBreedsCommandValidator : AbstractValidator<AddBreedsCommand>
{
    public AddBreedsCommandValidator()
    {
        RuleForEach(c => c.Names)
            .MustBeValueObject(Name.Create);
    }
}
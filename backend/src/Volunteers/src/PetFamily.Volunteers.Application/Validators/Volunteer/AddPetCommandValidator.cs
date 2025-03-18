using FluentValidation;
using PetFamily.Core;
using PetFamily.Core.Extensions;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Application.Commands.AddPet;
using PetFamily.Volunteers.Application.Validators.Shared;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;

namespace PetFamily.Volunteers.Application.Validators.Volunteer;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(v => v.VolunteerId)
            .MustBeValueObject(PetId.Create);
        RuleFor(v => v.Name)
            .MustBeValueObject(Name.Create);
        RuleFor(v => v.Classification)
            .MustBeValueObject(c => PetClassification.Create(c.SpeciesId, c.BreedId));
        RuleFor(v => v.Description)
            .MustBeValueObject(Description.Create);
        RuleFor(v => v.Color)
            .MustBeValueObject(Color.Create);
        RuleFor(v => v.HealthInfo)
            .MustBeValueObject(HealthInfo.Create);
        RuleFor(v => v.Address)
            .MustBeValueObject(a => Address.Create(a.City, a.Street, a.HouseNumber));
        RuleFor(v => v.Dimensions)
            .MustBeValueObject(d => Dimensions.Create(d.Height, d.Weight));
        RuleFor(v => v.OwnerPhone)
            .MustBeValueObject(Phone.Create);
        RuleForEach(v => v.TransferDetailDto)
            .SetValidator(new TransferDetailsValidator());
    }
}
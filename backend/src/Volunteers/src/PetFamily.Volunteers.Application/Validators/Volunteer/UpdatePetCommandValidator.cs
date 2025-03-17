using FluentValidation;
using PetFamily.Core;
using PetFamily.Core.Extensions;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Application.Commands.UpdatePet;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Validators.Volunteer;

public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator()
    {
        RuleFor(c => c.VolunteerId)
            .MustBeValueObject(VolunteerId.Create);
        RuleFor(c => c.PetId)
            .MustBeValueObject(PetId.Create);
        RuleFor(c => c.Name)
            .MustBeValueObject(Name.Create!)
            .When(c => c.Name != null);
        RuleFor(c => new {c.Classification.SpeciesId, c.Classification.BreedId})
            .MustBeValueObject(c => PetClassification.Create(c.SpeciesId, c.BreedId));
        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create!)
            .When(c => c.Description != null);
        RuleFor(c => c.Color)
            .MustBeValueObject(Color.Create!)
            .When(c => c.Color != null);
        RuleFor(c => c.HealthInfo)
            .MustBeValueObject(HealthInfo.Create!)
            .When(c => c.HealthInfo != null);
        RuleFor(c => new {c.Address!.City, c.Address.Street, c.Address.HouseNumber})
            .MustBeValueObject(c => Address.Create(c.City, c.Street, c.HouseNumber))
            .When(c => c.Address != null);
        RuleFor(c => new {c.Dimensions!.Height, c.Dimensions.Weight})
            .MustBeValueObject(c => Dimensions.Create(c.Height, c.Weight))
            .When(c => c.Dimensions != null);
        RuleFor(c => c.OwnerPhone)
            .MustBeValueObject(Phone.Create!)
            .When(c => c.OwnerPhone != null);
        RuleFor(c => c.HealthInfo)
            .MustBeValueObject(HealthInfo.Create!)
            .When(c => c.HealthInfo != null);
        RuleForEach(c => c.TransferDetailsDto)
            .MustBeValueObject(c => TransferDetails.Create(c.Name, c.Description))
            .When(c => c.TransferDetailsDto != null);
    }
}
using FluentValidation;
using PetFamily.Application.PetManagement.UseCases.AddPetPhotos;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

namespace PetFamily.Application.Validators.Pet;

public class AddPetPhotosCommandValidator : AbstractValidator<AddPetPhotosCommand>
{
    public AddPetPhotosCommandValidator()
    {
        RuleFor(c => c.VolunteerId)
            .MustBeValueObject(VolunteerId.Create);
        RuleFor(c => c.PetId)
            .MustBeValueObject(PetId.Create);
        RuleForEach(c => c.Photos)
            .NotEmpty()
            .NotNull(); //Fixme подумать над нормальной валидацией фото
    }
}
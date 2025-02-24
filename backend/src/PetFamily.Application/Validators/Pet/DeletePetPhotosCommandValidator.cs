using FluentValidation;
using PetFamily.Application.PetManagement.UseCases.DeletePetPhotos;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

namespace PetFamily.Application.Validators.Pet;

public class DeletePetPhotosCommandValidator : AbstractValidator<DeletePetPhotosCommand>
{
    public DeletePetPhotosCommandValidator()
    {
        RuleFor(c => c.VolunteerId)
            .MustBeValueObject(VolunteerId.Create);
        RuleFor(c => c.PetId)
            .MustBeValueObject(PetId.Create);
        RuleForEach(c => c.PhotoNames)
            .NotEmpty()
            .NotNull();
    }
}
using FluentValidation;
using PetFamily.Application.PetManagement.Commands.MainPetPhoto;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Validators.Pet;

public class PetMainPhotoCommandValidator : AbstractValidator<PetMainPhotoCommand>
{
    public PetMainPhotoCommandValidator()
    {
        RuleFor(c => c.VolunteerId)
            .MustBeValueObject(VolunteerId.Create);
        RuleFor(c => c.PetId)
            .MustBeValueObject(PetId.Create);
        RuleFor(c => new {c.FullPath})
            .MustBeValueObject(m => FilePath.Create(m.FullPath, null));
    }
}
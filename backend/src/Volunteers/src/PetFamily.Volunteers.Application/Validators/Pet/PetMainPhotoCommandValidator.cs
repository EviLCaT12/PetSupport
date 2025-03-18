using FluentValidation;
using PetFamily.Core;
using PetFamily.Core.Extensions;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Application.Commands.MainPetPhoto;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Validators.Pet;

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
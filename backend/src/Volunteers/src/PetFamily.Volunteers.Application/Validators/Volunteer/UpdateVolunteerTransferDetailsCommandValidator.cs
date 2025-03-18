using FluentValidation;
using PetFamily.Core;
using PetFamily.Core.Extensions;
using PetFamily.Core.Validations;
using PetFamily.Volunteers.Application.Commands.UpdateTransferDetails;
using PetFamily.Volunteers.Application.Validators.Shared;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Validators.Volunteer;

public class UpdateVolunteerTransferDetailsCommandValidator : AbstractValidator<UpdateVolunteerTransferDetailsCommand>
{
    public UpdateVolunteerTransferDetailsCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .MustBeValueObject(VolunteerId.Create);
        RuleForEach(u => u.NewTransferDetails)
            .SetValidator(new TransferDetailsValidator()); 
    }
}
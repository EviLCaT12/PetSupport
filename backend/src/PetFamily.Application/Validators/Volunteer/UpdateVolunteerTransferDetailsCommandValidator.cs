using FluentValidation;
using PetFamily.Application.PetManagement.Commands.UpdateTransferDetails;
using PetFamily.Application.Validators.Shared;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

namespace PetFamily.Application.Validators.Volunteer;

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
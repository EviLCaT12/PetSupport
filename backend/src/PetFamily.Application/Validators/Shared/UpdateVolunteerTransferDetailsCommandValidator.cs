using FluentValidation;
using PetFamily.Application.Volunteers.UpdateTransferDetails;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

namespace PetFamily.Application.Validators.Shared;

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
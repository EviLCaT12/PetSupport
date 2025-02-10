using FluentValidation;
using PetFamily.Application.Volunteers.HardDelete;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

namespace PetFamily.Application.Validators.Volunteer;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(d => d.VolunteerId)
            .MustBeValueObject(VolunteerId.Create);
    }
}
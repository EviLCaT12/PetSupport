using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerCommandValidator : AbstractValidator<UpdateVolunteerCommand>
{
    public UpdateVolunteerCommandValidator()
    {
        RuleFor(u => u.VolunteerId).MustBeValueObject(VolunteerId.Create);
    }
}
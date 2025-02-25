using FluentValidation;
using PetFamily.Application.PetManagement.Commands.UpdateSocialWeb;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

namespace PetFamily.Application.Validators.Volunteer;

public class UpdateVolunteerSocialWebValidator : AbstractValidator<UpdateVolunteerSocialWebCommand>
{
    public UpdateVolunteerSocialWebValidator()
    {
        RuleFor(u => u.VolunteerId)
            .MustBeValueObject(VolunteerId.Create);
        RuleForEach(u => u.NewSocialWebs)
            .SetValidator(new SocialWebValidator());
    }
}
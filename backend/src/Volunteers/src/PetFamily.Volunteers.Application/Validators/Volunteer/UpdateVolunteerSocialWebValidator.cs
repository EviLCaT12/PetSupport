using FluentValidation;
using PetFamily.Core;
using PetFamily.Core.Extensions;
using PetFamily.Core.Validations;
using PetFamily.Volunteers.Application.Commands.UpdateSocialWeb;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Validators.Volunteer;

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
using FluentValidation;
using PetFamily.Core.Dto.Shared;

namespace PetFamily.Volunteers.Application.Validators.Volunteer;

public class SocialWebListValidator : AbstractValidator<IEnumerable<SocialWebDto>>
{
    public SocialWebListValidator()
    {
        RuleForEach(socialWeb => socialWeb)
            .SetValidator(new SocialWebValidator());
    }
    
}
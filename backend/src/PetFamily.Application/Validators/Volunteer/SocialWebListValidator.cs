using FluentValidation;
using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.Validators.Volunteer;

public class SocialWebListValidator : AbstractValidator<IEnumerable<SocialWebDto>>
{
    public SocialWebListValidator()
    {
        RuleForEach(socialWeb => socialWeb)
            .SetValidator(new SocialWebValidator());
    }
    
}
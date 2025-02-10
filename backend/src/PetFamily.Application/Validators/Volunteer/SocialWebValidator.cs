using FluentValidation;
using PetFamily.Application.Dto.Shared;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

namespace PetFamily.Application.Validators.Volunteer;

public class SocialWebValidator : AbstractValidator<SocialWebDto>
{
    public SocialWebValidator()
    {
        RuleFor(socialWeb => new {socialWeb.Link, socialWeb.Name})
            .MustBeValueObject(dto => SocialWeb.Create(dto.Link, dto.Name));
    }
}
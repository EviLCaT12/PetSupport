using FluentValidation;
using PetFamily.Application.Dto.Shared;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

namespace PetFamily.Application.Validators;

public class CreateSocialWebValidator : AbstractValidator<SocialWebDto>
{
    public CreateSocialWebValidator()
    {
        RuleFor(socialWeb => new {socialWeb.Link, socialWeb.Name})
            .MustBeValueObject(dto => SocialWeb.Create(dto.Link, dto.Name));
    }
}
using FluentValidation;
using PetFamily.Core;
using PetFamily.Core.Dto.Shared;
using PetFamily.Core.Extensions;
using PetFamily.Core.Validations;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Validators.Volunteer;

public class SocialWebValidator : AbstractValidator<SocialWebDto>
{
    public SocialWebValidator()
    {
        RuleFor(socialWeb => new {socialWeb.Link, socialWeb.Name})
            .MustBeValueObject(dto => SocialWeb.Create(dto.Link, dto.Name));
    }
}
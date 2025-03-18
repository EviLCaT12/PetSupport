using FluentValidation;
using PetFamily.Core;
using PetFamily.Core.Extensions;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Application.Commands.Create;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Validators.Volunteer;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(v => v.Fio)
            .MustBeValueObject(x => VolunteerFio.Create(x.FirstName, x.LastName, x.SurName));
        RuleFor(v => v.PhoneNumber).MustBeValueObject(Phone.Create);
        RuleFor(v => v.Email).MustBeValueObject(Email.Create);
        RuleFor(v => v.YearsOfExperience).MustBeValueObject(YearsOfExperience.Create);
        RuleFor(v => v.Description).MustBeValueObject(Description.Create);
    }
}
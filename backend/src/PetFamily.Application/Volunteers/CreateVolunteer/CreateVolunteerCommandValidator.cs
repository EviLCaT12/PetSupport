using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

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
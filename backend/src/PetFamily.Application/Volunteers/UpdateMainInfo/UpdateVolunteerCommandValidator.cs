using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerCommandValidator : AbstractValidator<UpdateVolunteerCommand>
{
    public UpdateVolunteerCommandValidator()
    {
        RuleFor(u => u.VolunteerId).MustBeValueObject(VolunteerId.Create);
        RuleFor(u => new {u.FirstName, u.LastName, u.SurName})
            .MustBeValueObject(fio => VolunteerFio.Create(fio.FirstName, fio.LastName, fio.SurName));
        RuleFor(u => u.Phone).MustBeValueObject(Phone.Create);
        RuleFor(u => u.Email).MustBeValueObject(Email.Create);
        RuleFor(u => u.Description).MustBeValueObject(Description.Create);
        RuleFor(u => u.YearsOfExperience).MustBeValueObject(YearsOfExperience.Create);
    }
}
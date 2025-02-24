using FluentValidation;
using PetFamily.Application.PetManagement.UseCases.UpdateMainInfo;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Validators.Volunteer;

public class UpdateVolunteerMainInfoCommandValidator : AbstractValidator<UpdateVolunteerMainInfoCommand>
{
    public UpdateVolunteerMainInfoCommandValidator()
    {
        RuleFor(u => u.VolunteerId).MustBeValueObject(VolunteerId.Create);
        RuleFor(u => u.Fio)
            .MustBeValueObject(fio => VolunteerFio.Create(fio.FirstName, fio.LastName, fio.SurName));
        RuleFor(u => u.Phone).MustBeValueObject(Phone.Create);
        RuleFor(u => u.Email).MustBeValueObject(Email.Create);
        RuleFor(u => u.Description).MustBeValueObject(Description.Create);
        RuleFor(u => u.YearsOfExperience).MustBeValueObject(YearsOfExperience.Create);
    }
}
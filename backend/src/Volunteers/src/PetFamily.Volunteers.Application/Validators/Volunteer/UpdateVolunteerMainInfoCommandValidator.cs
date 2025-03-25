using FluentValidation;
using PetFamily.Core;
using PetFamily.Core.Extensions;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Application.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Validators.Volunteer;

public class UpdateVolunteerMainInfoCommandValidator : AbstractValidator<UpdateVolunteerMainInfoCommand>
{
    public UpdateVolunteerMainInfoCommandValidator()
    {
        RuleFor(u => u.VolunteerId).MustBeValueObject(VolunteerId.Create);
        RuleFor(u => u.Fio)
            .MustBeValueObject(fio => Fio.Create(fio.FirstName, fio.LastName, fio.SurName));
        RuleFor(u => u.Phone).MustBeValueObject(Phone.Create);
        RuleFor(u => u.Email).MustBeValueObject(Email.Create);
        RuleFor(u => u.Description).MustBeValueObject(Description.Create);
    }
}
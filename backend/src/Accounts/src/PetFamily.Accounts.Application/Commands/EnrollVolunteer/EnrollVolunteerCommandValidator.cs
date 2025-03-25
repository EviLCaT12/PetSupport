using FluentValidation;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Accounts.Application.Commands.EnrollVolunteer;

public class EnrollVolunteerCommandValidator : AbstractValidator<EnrollVolunteerCommand>
{
    public EnrollVolunteerCommandValidator()
    {
        RuleFor(c => c.PhoneNumber).MustBeValueObject(Phone.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        RuleFor(c => c.Experience).MustBeValueObject(YearsOfExperience.Create);
    }
}
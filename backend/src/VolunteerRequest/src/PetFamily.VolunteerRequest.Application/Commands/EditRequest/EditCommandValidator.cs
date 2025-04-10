using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Application.Commands.EditRequest;

public class EditCommandValidator : AbstractValidator<EditCommand>
{
    public EditCommandValidator()
    {
        RuleFor(c => c.RequestId).MustBeValueObject(VolunteerRequestId.Create);
        RuleFor(c => new { c.Fio.FirstName, c.Fio.LastName, c.Fio.SurName })
            .MustBeValueObject(x => Fio.Create(x.FirstName, x.LastName, x.SurName));
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        RuleFor(c => c.Experience).MustBeValueObject(YearsOfExperience.Create);
    }
}
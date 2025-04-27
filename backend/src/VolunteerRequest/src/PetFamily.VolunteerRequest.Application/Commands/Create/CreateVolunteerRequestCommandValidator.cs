using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.VolunteerRequest.Application.Commands.Create;

public class CreateVolunteerRequestCommandValidator : AbstractValidator<CreateVolunteerRequestCommand>
{
    public CreateVolunteerRequestCommandValidator()
    {
        RuleFor(c => new { c.FullName.FirstName, c.FullName.LastName, c.FullName.SurName })
            .MustBeValueObject(x => Fio.Create(x.FirstName, x.LastName, x.SurName));
        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);
        RuleFor(c => c.Email)
            .MustBeValueObject(Email.Create);
        RuleFor(c => c.Experience)
            .MustBeValueObject(YearsOfExperience.Create);
    }
}
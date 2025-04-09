using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Application.Commands.ApproveRequest;

public class ApproveRequestCommandValidator : AbstractValidator<ApproveRequestCommand>
{
    public ApproveRequestCommandValidator()
    {
        RuleFor(v => v.RequestId).MustBeValueObject(VolunteerRequestId.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(Phone.Create);
    }
}
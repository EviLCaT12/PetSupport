using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Application.Commands.RejectRequest;

public class RejectRequestCommandValidator : AbstractValidator<RejectRequestCommand>
{
    public RejectRequestCommandValidator()
    {
        RuleFor(c => c.RequestId).MustBeValueObject(VolunteerRequestId.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
    }
}
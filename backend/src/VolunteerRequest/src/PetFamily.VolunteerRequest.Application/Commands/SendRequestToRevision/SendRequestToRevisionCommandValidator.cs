using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Application.Commands.SendRequestToRevision;

public class SendRequestToRevisionCommandValidator : AbstractValidator<SendRequestToRevisionCommand>
{
    public SendRequestToRevisionCommandValidator()
    {
        RuleFor(c => c.RequestId).MustBeValueObject(VolunteerRequestId.Create);
        RuleFor(c => c.Discription).MustBeValueObject(Description.Create);
    }
}
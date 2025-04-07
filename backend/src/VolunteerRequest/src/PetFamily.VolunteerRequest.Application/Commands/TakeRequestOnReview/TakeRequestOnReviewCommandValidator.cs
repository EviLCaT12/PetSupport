using FluentValidation;
using PetFamily.Core.Validations;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Application.Commands.TakeRequestOnReview;

public class TakeRequestOnReviewCommandValidator : AbstractValidator<TakeRequestOnReviewCommand>
{
    public TakeRequestOnReviewCommandValidator()
    {
        RuleFor(request => request.RequestId).MustBeValueObject(VolunteerRequestId.Create);
        RuleFor(request => request.AdminId).NotEmpty().WithMessage("Id cannot be empty");
    }
}
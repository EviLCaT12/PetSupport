using FluentValidation;
using PetFamily.Core;
using PetFamily.Core.Extensions;
using PetFamily.Core.Validations;
using PetFamily.Volunteers.Application.Commands.ChangePetPosition;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;

namespace PetFamily.Volunteers.Application.Validators.Pet;

public class ChangePetPositionCommandValidator : AbstractValidator<ChangePetPositionCommand>
{
    public ChangePetPositionCommandValidator()
    {
        RuleFor(c => c.VolunteerId)
            .MustBeValueObject(PetId.Create);
        RuleFor(c => c.PetId)
            .MustBeValueObject(PetId.Create);
        RuleFor(c => c.PetPosition)
            .MustBeValueObject(Position.Create);
    }
}
using FluentValidation;
using PetFamily.Application.PetManagement.Commands.ChangePetPosition;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;

namespace PetFamily.Application.Validators.Pet;

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
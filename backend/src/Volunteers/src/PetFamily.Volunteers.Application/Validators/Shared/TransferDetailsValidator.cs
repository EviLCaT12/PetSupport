using FluentValidation;
using PetFamily.Core;
using PetFamily.Core.Dto.Shared;
using PetFamily.Core.Extensions;
using PetFamily.Core.Validations;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Volunteers.Application.Validators.Shared;

public class TransferDetailsValidator : AbstractValidator<TransferDetailDto>
{
    public TransferDetailsValidator()
    {
        RuleFor(transferDetails => new {transferDetails.Name, transferDetails.Description})
            .MustBeValueObject(dto => TransferDetails.Create(dto.Name, dto.Description));
    }
}
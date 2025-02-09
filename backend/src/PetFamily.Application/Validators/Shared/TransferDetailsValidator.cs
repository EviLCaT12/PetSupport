using FluentValidation;
using PetFamily.Application.Dto.Shared;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Validators.Shared;

public class TransferDetailsValidator : AbstractValidator<TransferDetailDto>
{
    public TransferDetailsValidator()
    {
        RuleFor(transferDetails => new {transferDetails.Name, transferDetails.Description})
            .MustBeValueObject(dto => TransferDetails.Create(dto.Name, dto.Description));
    }
}
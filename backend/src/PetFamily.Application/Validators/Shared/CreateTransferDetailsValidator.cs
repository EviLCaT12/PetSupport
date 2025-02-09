using FluentValidation;
using PetFamily.Application.Dto.Shared;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Validators.Shared;

public class CreateTransferDetailsValidator : AbstractValidator<TransferDetailDto>
{
    public CreateTransferDetailsValidator()
    {
        RuleFor(transferDetails => new {transferDetails.Name, transferDetails.Description})
            .MustBeValueObject(dto => TransferDetails.Create(dto.Name, dto.Description));
    }
}
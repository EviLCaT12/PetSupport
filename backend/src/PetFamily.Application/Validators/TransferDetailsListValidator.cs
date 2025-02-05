using FluentValidation;
using PetFamily.Application.Dto.Shared;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Validators;

public class TransferDetailsListValidator : AbstractValidator<IEnumerable<TransferDetailDto>>
{
    public TransferDetailsListValidator()
    {
        RuleForEach(transferDetails => transferDetails)
            .SetValidator(new CreateTransferDetailsValidator());
    }
}
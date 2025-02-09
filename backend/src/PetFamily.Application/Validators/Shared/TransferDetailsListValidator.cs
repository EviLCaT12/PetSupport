using FluentValidation;
using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.Validators.Shared;

public class TransferDetailsListValidator : AbstractValidator<IEnumerable<TransferDetailDto>>
{
    public TransferDetailsListValidator()
    {
        RuleForEach(transferDetails => transferDetails)
            .SetValidator(new TransferDetailsValidator());
    }
}
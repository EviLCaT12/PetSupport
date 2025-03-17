using FluentValidation;
using PetFamily.Core.Dto.Shared;

namespace PetFamily.Volunteers.Application.Validators.Shared;

public class TransferDetailsListValidator : AbstractValidator<IEnumerable<TransferDetailDto>>
{
    public TransferDetailsListValidator()
    {
        RuleForEach(transferDetails => transferDetails)
            .SetValidator(new TransferDetailsValidator());
    }
}
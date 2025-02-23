using PetFamily.Application.Dto.Shared;
using PetFamily.Application.Volunteers.UpdateTransferDetails;

namespace PetFamily.API.Requests.UpdateVolunteer;

public record UpdateVolunteerTransferDetailsRequest(IEnumerable<TransferDetailDto> NewTransferDetail)
{
    public UpdateVolunteerTransferDetailsCommand ToCommand(Guid volunteerId)
        => new UpdateVolunteerTransferDetailsCommand(
            volunteerId,
            NewTransferDetail);
}
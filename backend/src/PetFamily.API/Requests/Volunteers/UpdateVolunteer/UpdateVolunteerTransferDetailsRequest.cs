using PetFamily.Application.Dto.Shared;
using PetFamily.Application.PetManagement.Commands.UpdateTransferDetails;

namespace PetFamily.API.Requests.Volunteers.UpdateVolunteer;

public record UpdateVolunteerTransferDetailsRequest(IEnumerable<TransferDetailDto> NewTransferDetail)
{
    public UpdateVolunteerTransferDetailsCommand ToCommand(Guid volunteerId)
        => new UpdateVolunteerTransferDetailsCommand(
            volunteerId,
            NewTransferDetail);
}
using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.Volunteers.UpdateTransferDetails;

public record UpdateVolunteerTransferDetailsCommand(Guid VolunteerId, IEnumerable<TransferDetailDto> NewTransferDetails);
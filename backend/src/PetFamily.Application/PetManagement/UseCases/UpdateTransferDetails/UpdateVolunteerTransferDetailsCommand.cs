using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.PetManagement.UseCases.UpdateTransferDetails;

public record UpdateVolunteerTransferDetailsCommand(Guid VolunteerId, IEnumerable<TransferDetailDto> NewTransferDetails);
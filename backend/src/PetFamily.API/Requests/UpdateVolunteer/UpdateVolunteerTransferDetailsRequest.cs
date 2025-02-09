using PetFamily.Application.Dto.Shared;

namespace PetFamily.API.Requests.UpdateVolunteer;

public record UpdateVolunteerTransferDetailsRequest(IEnumerable<TransferDetailDto> NewTransferDetail);
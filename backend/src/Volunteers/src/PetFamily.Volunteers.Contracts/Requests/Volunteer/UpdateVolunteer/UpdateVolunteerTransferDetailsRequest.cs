using PetFamily.Core.Dto.Shared;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteer.UpdateVolunteer;

public record UpdateVolunteerTransferDetailsRequest(IEnumerable<TransferDetailDto> NewTransferDetail);
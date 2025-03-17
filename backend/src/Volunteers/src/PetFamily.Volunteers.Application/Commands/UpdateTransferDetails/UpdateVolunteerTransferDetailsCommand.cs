using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;

namespace PetFamily.Volunteers.Application.Commands.UpdateTransferDetails;

public record UpdateVolunteerTransferDetailsCommand(
    Guid VolunteerId, 
    IEnumerable<TransferDetailDto> NewTransferDetails) : ICommand;
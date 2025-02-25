using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.PetManagement.Commands.UpdateTransferDetails;

public record UpdateVolunteerTransferDetailsCommand(
    Guid VolunteerId, 
    IEnumerable<TransferDetailDto> NewTransferDetails) : ICommand;
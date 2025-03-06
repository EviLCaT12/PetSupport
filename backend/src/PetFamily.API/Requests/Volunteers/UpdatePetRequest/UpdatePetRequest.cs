using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Dto.Shared;
using PetFamily.Application.PetManagement.Commands.UpdatePet;

namespace PetFamily.API.Requests.Volunteers.UpdatePetRequest;

public record UpdatePetRequest(
    string? Name,
    PetClassificationDto Classification,
    string? Description,
    string? Color,
    string? HealthInfo,
    AddressDto? Address,
    DimensionsDto? Dimensions,
    string? OwnerPhone,
    bool? IsCastrate,
    DateTime? DateOfBirth,
    bool? IsVaccinated,
    int? HelpStatus,
    IEnumerable<TransferDetailDto>? TransferDetailsDto)
{
    public UpdatePetCommand ToCommand(Guid volunteerId, Guid petId)
        => new(
            volunteerId,
            petId,
            Name,
            Classification,
            Description,
            Color,
            HealthInfo,
            Address,
            Dimensions,
            OwnerPhone,
            IsCastrate,
            DateOfBirth,
            IsVaccinated,
            HelpStatus,
            TransferDetailsDto);
}

using PetFamily.Core.Dto.Shared;
using PetFamily.Volunteers.Contracts.Dto.PetDto;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteer.AddPet;

public record AddPetRequest(
    string Name,
    PetClassificationDto Classification,
    string Description,
    string Color,
    string HealthInfo,
    AddressDto Address,
    DimensionsDto Dimensions,
    string OwnerPhone,
    bool IsCastrate,
    DateTime DateOfBirth,
    bool IsVaccinated,
    string HelpStatus,
    IEnumerable<TransferDetailDto> TransferDetailsDto);
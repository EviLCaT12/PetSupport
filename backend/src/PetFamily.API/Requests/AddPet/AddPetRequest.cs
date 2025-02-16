using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Dto.Shared;

namespace PetFamily.API.Requests.AddPet;

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
    int HelpStatus,
    IEnumerable<TransferDetailDto> TransferDetailsDto);
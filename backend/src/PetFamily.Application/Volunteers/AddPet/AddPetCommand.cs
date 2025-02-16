using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.Volunteers.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
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
    IEnumerable<TransferDetailDto> TransferDetailDto);
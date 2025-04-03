using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;
using PetFamily.Volunteers.Contracts.Dto.PetDto;

namespace PetFamily.Volunteers.Application.Commands.AddPet;

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
    string HelpStatus,
    IEnumerable<TransferDetailDto> TransferDetailDto) : ICommand;
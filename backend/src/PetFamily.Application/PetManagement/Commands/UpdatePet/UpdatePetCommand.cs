using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.PetManagement.Commands.UpdatePet;

public record UpdatePetCommand(
    Guid VolunteerId,
    Guid PetId,
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
    IEnumerable<TransferDetailDto>? TransferDetailsDto) : ICommand;
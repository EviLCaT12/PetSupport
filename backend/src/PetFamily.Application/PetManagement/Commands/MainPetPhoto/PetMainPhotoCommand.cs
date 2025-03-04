using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.MainPetPhoto;

public record PetMainPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string FullPath) : ICommand;
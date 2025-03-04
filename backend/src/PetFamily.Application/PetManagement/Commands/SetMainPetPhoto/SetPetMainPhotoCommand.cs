using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.SetMainPetPhoto;

public record SetPetMainPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string FullPath) : ICommand;
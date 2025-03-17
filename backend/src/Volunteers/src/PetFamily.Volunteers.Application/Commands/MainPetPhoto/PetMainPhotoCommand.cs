using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Commands.MainPetPhoto;

public record PetMainPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string FullPath) : ICommand;
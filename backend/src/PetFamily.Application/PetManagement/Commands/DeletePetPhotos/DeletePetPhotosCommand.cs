using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.DeletePetPhotos;

public record DeletePetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<string> PhotoNames) : ICommand;
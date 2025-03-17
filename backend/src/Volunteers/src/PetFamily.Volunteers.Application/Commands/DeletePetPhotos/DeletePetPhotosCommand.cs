using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Commands.DeletePetPhotos;

public record DeletePetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<string> PhotoNames) : ICommand;
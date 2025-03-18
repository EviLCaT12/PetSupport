using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.RemoveBreed;

public record RemoveBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.RemoveBreed;

public record RemoveBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
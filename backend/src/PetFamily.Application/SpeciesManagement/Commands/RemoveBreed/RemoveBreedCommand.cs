using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.Commands.RemoveBreed;

public record RemoveBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
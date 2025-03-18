using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.Remove;

public record RemoveSpeciesCommand(Guid SpeciesId) : ICommand;
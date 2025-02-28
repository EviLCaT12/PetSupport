using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.Commands.Remove;

public record RemoveSpeciesCommand(Guid SpeciesId) : ICommand;
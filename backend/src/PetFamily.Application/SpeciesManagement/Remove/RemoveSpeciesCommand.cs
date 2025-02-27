using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.Remove;

public record RemoveSpeciesCommand(Guid SpeciesId) : ICommand;
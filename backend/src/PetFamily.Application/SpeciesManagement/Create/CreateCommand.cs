using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.Create;

public record CreateCommand(string Name) : ICommand;
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.Commands.Create;

public record CreateCommand(string Name) : ICommand;
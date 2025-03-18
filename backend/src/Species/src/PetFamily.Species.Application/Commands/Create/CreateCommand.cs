using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.Create;

public record CreateCommand(string Name) : ICommand;
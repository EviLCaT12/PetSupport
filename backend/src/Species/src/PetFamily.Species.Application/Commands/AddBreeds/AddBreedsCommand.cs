using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.AddBreeds;

public record AddBreedsCommand(Guid SpeciesId,IEnumerable<string> Names) : ICommand;
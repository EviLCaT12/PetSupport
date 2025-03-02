using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.Commands.AddBreeds;

public record AddBreedsCommand(Guid SpeciesId,IEnumerable<string> Names) : ICommand;
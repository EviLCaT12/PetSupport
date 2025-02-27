using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.AddBreeds;

public record AddBreedsCommand(Guid SpeciesId,IEnumerable<string> Names) : ICommand;
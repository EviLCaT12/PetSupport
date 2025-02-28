using PetFamily.Application.Dto.BreedDto;
using PetFamily.Application.SpeciesManagement.Commands.AddBreeds;

namespace PetFamily.API.Requests.Species;

public record AddBreedsRequest(IEnumerable<string> Names)
{
    public AddBreedsCommand ToCommand(Guid id) => new(id, Names);
}
using PetFamily.Application.SpeciesManagement.Create;

namespace PetFamily.API.Requests.Species;

public record CreateRequest(string Name)
{
    public CreateCommand ToCommand() => new (Name);
}
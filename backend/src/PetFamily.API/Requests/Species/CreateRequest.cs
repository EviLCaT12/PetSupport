using PetFamily.Application.SpeciesManagement.Commands.Create;

namespace PetFamily.API.Requests.Species;

public record CreateRequest(string Name)
{
    public CreateCommand ToCommand() => new (Name);
}
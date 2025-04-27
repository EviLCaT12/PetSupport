using PetFamily.Core.Abstractions;

namespace PetFamily.Discussion.Application.Commands.Create;

public record CreateCommand(Guid RelationId, IEnumerable<Guid> Members) : ICommand;

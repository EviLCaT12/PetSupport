using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Queries.GerUserById;

public record GetUserByIdQuery(Guid UserId) : IQuery;
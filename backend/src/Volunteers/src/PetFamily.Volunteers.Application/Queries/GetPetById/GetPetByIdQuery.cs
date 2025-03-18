using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Queries.GetPetById;

public record GetPetByIdQuery(Guid id) : IQuery;
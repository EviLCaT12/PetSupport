using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.GetPetById;

public record GetPetByIdQuery(Guid id) : IQuery;
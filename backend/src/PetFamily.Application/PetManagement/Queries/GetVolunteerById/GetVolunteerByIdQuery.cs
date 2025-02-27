using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid Id) : IQuery;
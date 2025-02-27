using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dto.VolunteerDto;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.PetManagement.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationHandler : IQueryHandler<PagedList<VolunteerDto>, GetVolunteerWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetVolunteersWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> HandleAsync(GetVolunteerWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var volunteerQuery = _context.Volunteers;

        return await volunteerQuery.OrderBy(v => v.Id).ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}
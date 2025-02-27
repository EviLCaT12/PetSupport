using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dto.VolunteerDto;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers;

namespace PetFamily.Application.PetManagement.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationHandler : IQueryHandler<PagedList<VolunteerDto>, GetVolunteerWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetVolunteersWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<VolunteerDto>> HandleAsync(GetVolunteerWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var volunteerQuery = _context.Volunteers;

        return await volunteerQuery.OrderBy(v => v.Id).ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}
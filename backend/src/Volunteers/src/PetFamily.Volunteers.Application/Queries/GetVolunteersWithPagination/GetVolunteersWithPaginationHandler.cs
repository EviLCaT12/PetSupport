using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel.Error;
using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

namespace PetFamily.Volunteers.Application.Queries.GetVolunteersWithPagination;

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
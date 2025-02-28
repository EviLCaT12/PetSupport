using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dto.SpeciesDto;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.SpeciesManagement.Queries.GetSpeciesWithPagination;

public class GetSpeciesWithPaginationHandler : IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetSpeciesWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }
    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> HandleAsync(
        GetSpeciesWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var species = _context.Species;

        return await species
            .OrderBy(s => s.Name)
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}
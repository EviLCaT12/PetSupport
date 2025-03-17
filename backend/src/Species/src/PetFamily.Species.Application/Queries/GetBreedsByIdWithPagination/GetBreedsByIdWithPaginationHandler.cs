using CSharpFunctionalExtensions;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.BreedDto;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Species.Application.Queries.GetBreedsByIdWithPagination;

public class GetBreedsByIdWithPaginationHandler : IQueryHandler<PagedList<BreedDto>, GetBreedsByIdWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetBreedsByIdWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }
    public async Task<Result<PagedList<BreedDto>, ErrorList>> HandleAsync(
        GetBreedsByIdWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var breeds = _context.Breeds;

        return await breeds
            .Where(b => b.SpeciesId == query.SpeciesId)
            .OrderBy(b => b.Name)
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}
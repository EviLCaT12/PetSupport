using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dto.BreedDto;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.SpeciesManagement.Queries.GetBreedsByIdWithPagination;

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
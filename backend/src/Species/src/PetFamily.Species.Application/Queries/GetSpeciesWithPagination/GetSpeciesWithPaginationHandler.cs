using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.SpeciesDto;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Species.Application.Queries.GetSpeciesWithPagination;

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
            .Include(s => s.Breeds)
            .OrderBy(s => s.Name)
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}
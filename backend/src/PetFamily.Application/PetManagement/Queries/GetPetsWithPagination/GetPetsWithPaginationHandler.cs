using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

public class GetPetsWithPaginationHandler : IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationQuery>
{
    private readonly IReadDbContext _context;
    
    public GetPetsWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PagedList<PetDto>, ErrorList>> HandleAsync
        (GetPetsWithPaginationQuery query,
            CancellationToken ct)
    {
        var petQuery = _context.Pets;
        
        return await petQuery.OrderBy(p => p.Position).ToPagedList(query.Page, query.PageSize, ct);
    }
}
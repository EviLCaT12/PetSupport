using PetFamily.Application.DataBase;
using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers;

namespace PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

public class GetPetsWithPaginationHandler
{
    private readonly IReadDbContext _context;


    public GetPetsWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<PetDto>> HandleAsync
        (GetPetsWithPaginationQuery query, CancellationToken ct)
    {
        var petQuery = _context.Pets.AsQueryable();
        
        return await petQuery.ToPagedList(query.Page, query.PageSize, ct);
    }
}
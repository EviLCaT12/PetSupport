using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.PetDto;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Volunteers.Application.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly IReadDbContext _context;

    public GetPetByIdHandler(IReadDbContext context)
    {
        _context = context;
    }
    public async Task<Result<PetDto, ErrorList>> HandleAsync(GetPetByIdQuery query, CancellationToken cancellationToken)
    {
        var petQuery = _context.Pets;
        
        var pet = await petQuery.FirstOrDefaultAsync(p => p.Id == query.id, cancellationToken);

        return pet;
    }
}
using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.PetDto;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Volunteers.Application.Queries.GetPetsWithPagination;

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

        Expression<Func<PetDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "name" => (pet) => pet.Name,
            "dateofbirth" => (pet) => pet.DateOfBirth,
            "breedid" => (pet) => pet.BreedId,
            "color" => (pet) => pet.Color,
            "city" => (pet) => pet.City,
            "volunteerid" => (pet) => pet.VolunteerId,
            _ => (pet) => pet.Id
        };

        petQuery = query.SortDirection?.ToLower() == "desc"
            ? petQuery.OrderByDescending(keySelector)
            : petQuery.OrderBy(keySelector);
        
        petQuery = petQuery.WhereIf(
            query.VolunteerId != Guid.Empty && query.VolunteerId != null,
            p => p.VolunteerId == query.VolunteerId);
        
        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Name),
            p => p.Name.Contains(query.Name!));
        
        petQuery = petQuery.WhereIf(
            query.PositionFrom != null,
            p => p.Position >= query.PositionFrom);
        
        petQuery = petQuery.WhereIf(
            query.PositionTo != null,
            p => p.Position < query.PositionTo);
        
        petQuery = petQuery.WhereIf(
            query.SpeciesId != Guid.Empty && query.SpeciesId != null,
            p => p.SpeciesId == query.SpeciesId);
        
        petQuery = petQuery.WhereIf(
            query.BreedId != Guid.Empty && query.BreedId != null,
            p => p.BreedId == query.BreedId);
        
        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Color),
            p => p.Color.Contains(query.Color!));
        
        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.City),
            p => p.City.Contains(query.City!));
        
        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Street),
            p => p.Street.Contains(query.Street!));
        
        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.HouseNumber),
            p => p.HouseNumber.Contains(query.HouseNumber!));
        
        petQuery = petQuery.WhereIf(
            query.HeightFrom != null,
            p => p.Height >= query.HeightFrom);
        
        petQuery = petQuery.WhereIf(
            query.HeightTo != null,
            p => p.Height < query.HeightTo);
        
        petQuery = petQuery.WhereIf(
            query.WeightFrom != null,
            p => p.Weight >= query.WeightFrom);
        
        petQuery = petQuery.WhereIf(
            query.WeightTo != null,
            p => p.Weight < query.WeightTo);
        
        petQuery = petQuery.WhereIf(
            query.IsCastrated != null,
            p => p.IsCastrate == query.IsCastrated);
        
        petQuery = petQuery.WhereIf(
            query.Younger != null,
            p => p.DateOfBirth > query.Younger);
        
        petQuery = petQuery.WhereIf(
            query.Older != null,
            p => p.DateOfBirth < query.Older);
        
        petQuery = petQuery.WhereIf(
            query.IsVaccinated != null,
            p => p.IsVaccinated == query.IsVaccinated);
        
        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.HelpStatus),
            p => p.HelpStatus.Contains(query.HelpStatus!));
        
        return await petQuery
            .ToPagedList(query.Page, query.PageSize, ct);
    }
}
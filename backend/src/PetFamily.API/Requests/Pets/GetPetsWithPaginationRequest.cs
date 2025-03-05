using PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

namespace PetFamily.API.Requests.Pets;

public record GetPetsWithPaginationRequest(
    Guid? VolunteerId,
    string? Name,
    int? PositionFrom,
    int? PositionTo,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Color,
    string? City,
    string? Street,
    string? HouseNumber,
    float? HeightFrom,
    float? HeightTo,
    float? WeightFrom,
    float? WeightTo,
    bool? IsCastrated,
    DateTime? Younger,
    DateTime? Older,
    bool? IsVaccinated,
    string? HelpStatus,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetPetsWithPaginationQuery ToQuery() =>
        new (
            VolunteerId,
            Name,
            PositionFrom,
            PositionTo,
            SpeciesId,
            BreedId,
            Color,
            City,
            Street,
            HouseNumber,
            HeightFrom,
            HeightTo,
            WeightFrom,
            WeightTo,
            IsCastrated,
            Younger,
            Older,
            IsVaccinated,
            HelpStatus,
            SortBy,
            SortDirection,
            Page,
            PageSize);
}
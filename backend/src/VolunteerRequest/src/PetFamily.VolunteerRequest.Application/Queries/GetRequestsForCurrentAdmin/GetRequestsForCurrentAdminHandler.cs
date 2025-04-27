using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel.Error;
using PetFamily.VolunteerRequest.Application.Abstractions;
using PetFamily.VolunteerRequest.Contracts.Dto;
using PetFamily.VolunteerRequest.Domain.Enums;

namespace PetFamily.VolunteerRequest.Application.Queries.GetRequestsForCurrentAdmin;

public class GetRequestsForCurrentAdminHandler : IQueryHandler<PagedList<VolunteerRequestDto>,GetRequestsForCurrentAdminQuery>
{
    private readonly IReadDbContext _context;

    public GetRequestsForCurrentAdminHandler(IReadDbContext context)
    {
        _context = context;
    }
    public async Task<Result<PagedList<VolunteerRequestDto>, ErrorList>> HandleAsync(GetRequestsForCurrentAdminQuery query, CancellationToken cancellationToken)
    {
        var requestQuery = _context.VolunteerRequests
            .Where(r => r.AdminId == query.AdminId);

        if (!Enum.TryParse<Status>(query.Status, true, out var statusFilter))
            return Errors.VolunteerRequest.InvalidStatus();

        requestQuery = statusFilter switch
        {
            Status.Rejected => requestQuery.Where(r => r.Status == Status.Rejected),
            Status.RevisionRequired => requestQuery.Where(r => r.Status == Status.RevisionRequired),
            Status.Approved => requestQuery.Where(r => r.Status == Status.Approved),
            Status.Submitted => requestQuery.Where(r => r.Status == Status.Submitted),
            _ => requestQuery.Where(r => r.Status == Status.OnReview)
        };
        
        return await requestQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}
using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel.Error;
using PetFamily.VolunteerRequest.Application.Abstractions;
using PetFamily.VolunteerRequest.Contracts.Dto;
using PetFamily.VolunteerRequest.Domain.Enums;

namespace PetFamily.VolunteerRequest.Application.Queries.GetAllSubmittedRequestsWithPagination;

public class GetAllSubmittedRequestHandler : IQueryHandler<PagedList<VolunteerRequestDto>, GetAllSubmittedRequestQuery>
{
    private readonly IReadDbContext _context;

    public GetAllSubmittedRequestHandler(IReadDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<PagedList<VolunteerRequestDto>, ErrorList>> HandleAsync(GetAllSubmittedRequestQuery query,
        CancellationToken cancellationToken)
    {
        var requestsQuery = _context.VolunteerRequests
            .Where(r => r.Status == Status.Submitted);
        
        return await requestsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}
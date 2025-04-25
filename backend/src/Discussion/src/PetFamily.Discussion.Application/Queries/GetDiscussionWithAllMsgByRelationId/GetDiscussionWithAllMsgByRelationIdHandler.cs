using Contracts.Dtos;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Discussion.Application.Database;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Discussion.Application.Queries.GetDiscussionWIthAllMsgByRelationId;

public class GetDiscussionWithAllMsgByRelationIdHandler : IQueryHandler<DiscussionDto, GetDiscussionWithAllMsgByRelationIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetDiscussionWithAllMsgByRelationIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<DiscussionDto, ErrorList>> HandleAsync(GetDiscussionWithAllMsgByRelationIdQuery query, CancellationToken cancellationToken)
    {
        var discussions = _readDbContext.Discussions;

        var result =  await discussions
            .Include(d => d.Messages)
            .FirstOrDefaultAsync(d => d.RelationId == query.RelationId, cancellationToken);

        if (result is null)
            return Errors.General.ValueNotFound(query.RelationId).ToErrorList();
        
        return result;
    }
}
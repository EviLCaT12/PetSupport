using Contracts;
using Contracts.Requests;
using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Discussion.Application.Commands.Create;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Discussion.Presentation;

public class DiscussionContract : IDiscussionContract
{
    private readonly ICommandHandler<Guid, CreateCommand> _handler;

    public DiscussionContract(ICommandHandler<Guid, CreateCommand> handler)
    {
        _handler = handler;
    }
    public async Task<Result<Guid, ErrorList>> CreateDiscussionForVolunteerRequest(CreateDiscussionRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateCommand(request.RequestId, request.MembersId);
        var discussion = await _handler.HandleAsync(command, cancellationToken);
        
        if (discussion.IsFailure)
            return discussion.Error;

        return discussion.Value;
    }
}
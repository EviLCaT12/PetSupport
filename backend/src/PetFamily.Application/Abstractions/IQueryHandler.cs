using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    // public Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
    
    public Task<Result<TResponse, ErrorList>> HandleAsync(TQuery query, CancellationToken cancellationToken);
}

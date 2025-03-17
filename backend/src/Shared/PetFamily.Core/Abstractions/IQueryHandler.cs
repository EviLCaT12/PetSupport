using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Core.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    // public Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
    
    public Task<Result<TResponse, ErrorList>> HandleAsync(TQuery query, CancellationToken cancellationToken);
}

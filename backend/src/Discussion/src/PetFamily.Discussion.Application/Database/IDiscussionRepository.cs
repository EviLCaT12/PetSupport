using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Application.Database;

public interface IDiscussionRepository
{
    Task AddAsync(Domain.Entities.Discussion discussion, CancellationToken cancellationToken = default);

    Task<Domain.Entities.Discussion?> GetByIdAsync(DiscussionsId id,
        CancellationToken cancellationToken = default);
}
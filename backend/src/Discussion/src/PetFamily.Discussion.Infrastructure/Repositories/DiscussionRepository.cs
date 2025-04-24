using Microsoft.EntityFrameworkCore;
using PetFamily.Discussion.Application.Database;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.Discussion.Infrastructure.Contexts;

namespace PetFamily.Discussion.Infrastructure.Repositories;

public class DiscussionRepository : IDiscussionRepository
{
    private readonly WriteDbContext _dbContext;

    public DiscussionRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Domain.Entities.Discussion discussion, CancellationToken cancellationToken = default)
    {
        await _dbContext.Discussions.AddAsync(discussion, cancellationToken);
    }

    public async Task<Domain.Entities.Discussion?> GetByIdAsync(DiscussionsId id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Discussions.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }
}
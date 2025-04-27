using Microsoft.EntityFrameworkCore;
using PetFamily.Discussion.Application.Database;
using PetFamily.Discussion.Domain.Entities;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.Discussion.Infrastructure.Contexts;

namespace PetFamily.Discussion.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly WriteDbContext _context;

    public MessageRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Message message, CancellationToken cancellationToken = default)
    {
        await _context.Messages.AddAsync(message, cancellationToken);
    }

    public async Task<Message?> GetByIdAsync(MessageId id, CancellationToken cancellationToken = default)
    {
        return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public void Delete(Message message)
    {
        _context.Messages.Remove(message);
    }
}
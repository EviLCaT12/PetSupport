using PetFamily.Discussion.Domain.Entities;
using PetFamily.Discussion.Domain.ValueObjects;

namespace PetFamily.Discussion.Application.Database;

public interface IMessageRepository
{
    Task AddAsync(Message message, CancellationToken cancellationToken = default);

    Task<Message?> GetByIdAsync(MessageId id, CancellationToken cancellationToken = default);

    void Delete(Message message);
}
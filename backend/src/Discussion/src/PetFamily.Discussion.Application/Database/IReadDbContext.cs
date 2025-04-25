using Contracts.Dtos;

namespace PetFamily.Discussion.Application.Database;

public interface IReadDbContext
{
    IQueryable<DiscussionDto> Discussions { get; }
    
    IQueryable<MessageDto> Messages { get; }
}
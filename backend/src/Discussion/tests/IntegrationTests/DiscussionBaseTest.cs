using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Discussion.Domain.Entities;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.Discussion.Infrastructure.Contexts;

namespace IntegrationTests;

public class DiscussionBaseTest : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsWebFactory Factory;
    protected readonly IServiceScope Scope;
    protected readonly WriteDbContext WriteDbContext;
    protected readonly Fixture Fixture;

    public DiscussionBaseTest(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        Fixture = new Fixture();
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    async Task IAsyncLifetime.DisposeAsync()
    {
        await Factory.ResetDatabaseAsync();
        Scope.Dispose();
    }

    public Discussion SeedDiscussionsAsync()
    {
        var discussionId = DiscussionsId.NewId();

        var relationId = Guid.NewGuid();
        
        var member1 = Guid.NewGuid();
        var member2 = Guid.NewGuid();
        List<Guid> members= [];
        members.Add(member1);
        members.Add(member2);
        
        var discussion = Discussion.Create(discussionId, relationId, members).Value;
        
        WriteDbContext.Discussions.Add(discussion);
        
        WriteDbContext.SaveChanges();
        
        return discussion;
    }

    public Message SeedMessage(Discussion discussion)
    {
        var messageId = MessageId.NewId();
        var userId = discussion.Members[0];
        var text = Text.Create("Hello World").Value;
        
        var message = new Message(messageId, userId, text);
        
        WriteDbContext.Messages.Add(message);
        
        WriteDbContext.SaveChanges();
        
        return message;
    }
}
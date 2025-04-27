using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Discussion.Domain.Entities;

namespace PetFamily.Discussion.Infrastructure.Contexts;

public class WriteDbContext(string connectionString) : DbContext
{
    public DbSet<Domain.Entities.Discussion> Discussions => Set<Domain.Entities.Discussion>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql(connectionString);
        builder.UseSnakeCaseNamingConvention();
        builder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
        
        builder.HasDefaultSchema("discussion");
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PetFamily.VolunteerRequest.Infrastructure.DbContexts;

public class WriteContext(string connectionString) : DbContext
{
    public DbSet<Domain.Entities.VolunteerRequest> VolunteerRequests => Set<Domain.Entities.VolunteerRequest>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteContext).Assembly, 
            type => type.FullName?.Contains("Configurations.Write") ?? false);
        
        modelBuilder.HasDefaultSchema("volunteer_request");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}
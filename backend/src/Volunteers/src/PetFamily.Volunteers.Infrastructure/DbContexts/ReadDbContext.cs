using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Contracts.Dto.PetDto;
using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

namespace PetFamily.Volunteer.Infrastructure.DbContexts;

public class ReadDbContext(string connectionString) : DbContext, IReadDbContext
{
    public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
    public IQueryable<PetDto> Pets => Set<PetDto>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ReadDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
        modelBuilder.HasDefaultSchema("volunteer");
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}
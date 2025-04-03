using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Species.Application;
using PetFamily.Species.Contracts.Dto.BreedDto;
using PetFamily.Species.Contracts.Dto.SpeciesDto;

namespace PetFamily.Species.Infrastructure.DbContexts;

public class ReadDbContext(string connectionString) : DbContext, IReadDbContext
{
    public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();
    public IQueryable<BreedDto> Breeds => Set<BreedDto>();
    
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
        modelBuilder.HasDefaultSchema("species");
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}
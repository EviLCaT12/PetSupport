using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.DataBase;
using PetFamily.Infrastructure.DbContexts;
using Testcontainers.PostgreSql;

namespace IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_family_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
        
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }


    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        var writeContext = services.FirstOrDefault(s =>
            s.ServiceType == typeof(WriteDbContext));
        
        var readDbContext = services.FirstOrDefault(s =>
            s.ServiceType == typeof(IReadDbContext));
        
        if (writeContext != null)
            services.Remove(writeContext);
        
        if (readDbContext != null)
            services.Remove(readDbContext);
        
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(_dbContainer.GetConnectionString()));
        
        services.AddScoped<IReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(_dbContainer.GetConnectionString()));
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    { 
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.API.Extensions;

public static class AppExtensions
{
    public static async Task<WebApplication> ApplyMigration(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbDbContext>();

        await dbContext.Database.MigrateAsync();

        return app;
    }
}
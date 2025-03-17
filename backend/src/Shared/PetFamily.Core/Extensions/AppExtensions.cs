// using Microsoft.AspNetCore.Builder;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace PetFamily.Core.Extensions;
//
// public static class AppExtensions
// {
//     public static async Task<WebApplication> ApplyMigration(this WebApplication app)
//     {
//         await using var scope = app.Services.CreateAsyncScope();
//         var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
//
//         await dbContext.Database.MigrateAsync();
//
//         return app;
//     }
// }
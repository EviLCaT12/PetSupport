using Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.Discussion.Presentation;

public static class DependencyInjection 
{
    public static IServiceCollection AddDiscussionPresentation(this IServiceCollection services)
    {
        services.AddScoped<IDiscussionContract, DiscussionContract>();
        return services;
    }
}
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Species.Contracts;
using PetFamily.Species.Presentation.Species;

namespace PetFamily.Species.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesPresentation(this IServiceCollection services)
    {
        services.AddScoped<ISpeciesContract, SpeciesContract>();
        return services;
    }
}
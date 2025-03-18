using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteer.Api.Pets;
using PetFamily.Volunteer.Api.Volunteers;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Volunteer.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerPresentation(this IServiceCollection services)
    {
        services.AddScoped<IPetContract, PetContract>();
        services.AddScoped<IVolunteerContract, VolunteerContract>();
        return services;
    }
}
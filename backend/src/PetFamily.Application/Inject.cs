using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPetPhotos;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.HardDelete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialWeb;
using PetFamily.Application.Volunteers.UpdateTransferDetails;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateVolunteerMainInfoHandler>();
        services.AddScoped<UpdateVolunteerSocialWebHandler>();
        services.AddScoped<UpdateVolunteerTransferDetailsHandler>();
        services.AddScoped<HardDeleteVolunteerHandler>();
        services.AddScoped<SoftDeleteVolunteerHandler>();
        services.AddScoped<AddPetHandler>();
        services.AddScoped<AddPetPhotosHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}
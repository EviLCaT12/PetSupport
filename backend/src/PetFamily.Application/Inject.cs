using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;
using PetFamily.Application.PetManagement.UseCases.AddPet;
using PetFamily.Application.PetManagement.UseCases.AddPetPhotos;
using PetFamily.Application.PetManagement.UseCases.ChangePetPosition;
using PetFamily.Application.PetManagement.UseCases.Create;
using PetFamily.Application.PetManagement.UseCases.DeletePetPhotos;
using PetFamily.Application.PetManagement.UseCases.HardDelete;
using PetFamily.Application.PetManagement.UseCases.UpdateMainInfo;
using PetFamily.Application.PetManagement.UseCases.UpdateSocialWeb;
using PetFamily.Application.PetManagement.UseCases.UpdateTransferDetails;

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
        services.AddScoped<DeletePetPhotosHandler>();
        services.AddScoped<ChangePetPositionHandler>();
        services.AddScoped<GetPetsWithPaginationHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}
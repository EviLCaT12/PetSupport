using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.AddPet;
using PetFamily.Application.PetManagement.Commands.AddPetPhotos;
using PetFamily.Application.PetManagement.Commands.ChangePetPosition;
using PetFamily.Application.PetManagement.Commands.Create;
using PetFamily.Application.PetManagement.Commands.DeletePetPhotos;
using PetFamily.Application.PetManagement.Commands.HardDelete;
using PetFamily.Application.PetManagement.Commands.UpdateMainInfo;
using PetFamily.Application.PetManagement.Commands.UpdateSocialWeb;
using PetFamily.Application.PetManagement.Commands.UpdateTransferDetails;
using PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddCommands()
            .AddQueries()
            .AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    } 

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
    
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
}
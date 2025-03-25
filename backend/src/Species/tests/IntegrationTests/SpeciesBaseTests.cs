using System.Globalization;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Dto.Shared;
using PetFamily.Core.Dto.VolunteerDto;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Species.Application;
using PetFamily.Species.Domain.Entities;
using PetFamily.Species.Domain.ValueObjects.BreedVO;
using PetFamily.Species.Domain.ValueObjects.SpeciesVO;
using PetFamily.Species.Infrastructure.DbContexts;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.CreateVolunteer;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;

namespace IntegrationTests;

public class SpeciesBaseTest : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly Fixture Fixture;
    protected readonly WriteDbContext WriteContext;
    protected readonly IReadDbContext ReadContext;
    protected readonly IServiceScope Scope;
    protected readonly IntegrationTestsWebFactory Factory;
    protected readonly IVolunteerContract Contract;
    
    protected SpeciesBaseTest(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        ReadContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        WriteContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        Fixture = new Fixture();
        Contract = Scope.ServiceProvider.GetRequiredService<IVolunteerContract>();
    }
    
    public async Task<Guid> SeedVolunteerAsync()
    {
        var fio = new FioDto("string", "string", "string");
        var phone = "+7 (123) 123-12-21";
        var email = "email@email.com";
        var description = "description";
        var exp = 12;
        List<SocialWebDto> socialWebsList = [];
        List<TransferDetailDto> transferDetails = [];
        
        var request = new CreateVolunteerRequest(
            fio, 
            phone, 
            email,
            description);
        
        var id = await Contract.AddVolunteer(request, CancellationToken.None);
        return id.Value;
    }

    public async Task<Guid> SeedPetAsync(Volunteer volunteer, Guid? speciesId = null, Guid? breedId = null)
    {
        var id = PetId.NewPetId();
        var name = Name.Create("name").Value;
        var classification = PetClassification
            .Create(speciesId ?? Guid.NewGuid(), breedId ?? Guid.NewGuid()).Value; 
        var description = Description.Create("description").Value;
        var color = Color.Create("color").Value;
        var healthInfo = HealthInfo.Create("health").Value;
        var address = Address.Create("string", "string", "string").Value;
        var dimensions = Dimensions.Create(1, 1).Value;
        var phone = Phone.Create("+7 (123) 123-12-21").Value;
        var isCastrate = true;
        DateTime dateOfBirth = DateTime.Parse(
            "2025-03-12T13:13:14.384Z",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal);
        var isVaccinated = true;
        var helpStatus = "SeekHome";
        IEnumerable<TransferDetails> transferDetailsList = [];
        IEnumerable<Photo> photoList = [];
        
        var pet = Pet.Create(
            id, name, classification, description, color, 
            healthInfo, address, dimensions, phone, isCastrate,
            dateOfBirth, isVaccinated, helpStatus, transferDetailsList, photoList).Value;

        volunteer.AddPet(pet);
        
        await WriteContext.SaveChangesAsync(CancellationToken.None);
        
        return id.Value;
    }

    public async Task<Guid> SeedSpeciesAsync()
    {
        var speciesId = SpeciesId.NewSpeciesId();
        var speciesName = Name.Create("name").Value;
        
        var species = Species.Create(speciesId, speciesName).Value;
        
        await WriteContext.Species.AddAsync(species);
        await WriteContext.SaveChangesAsync(CancellationToken.None);

        return species.Id.Value;
    }

    public async Task<Guid> SeedBreedAsync(Species species)
    {
        var breedId = BreedId.NewBreedId();
        var breedName = Name.Create("Test").Value;
        var breed = Breed.Create(breedId, breedName).Value;
        
        species.AddBreeds([breed]);
        await WriteContext.SaveChangesAsync(CancellationToken.None);
        
        return breedId.Value;
    } 

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await Factory.ResetDatabaseAsync();
    }
}
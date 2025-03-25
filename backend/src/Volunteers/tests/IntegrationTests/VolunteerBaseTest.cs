using System.Globalization;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests.Species;
using PetFamily.Species.Domain.Entities;
using PetFamily.Species.Domain.ValueObjects.BreedVO;
using PetFamily.Volunteer.Infrastructure.DbContexts;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;



namespace IntegrationTests;   

public class VolunteerBaseTest : IClassFixture<TestsWebFactory>, IAsyncLifetime
{
    protected readonly Fixture Fixture;
    protected readonly WriteDbContext WriteContext;
    protected readonly IReadDbContext ReadContext;
    protected readonly IServiceScope Scope;
    protected readonly TestsWebFactory Factory;
    protected readonly ISpeciesContract Contract;

    protected VolunteerBaseTest(TestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        ReadContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        WriteContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        Contract = Scope.ServiceProvider.GetRequiredService<ISpeciesContract>();
        Fixture = new Fixture();
    }
    
    public async Task<Guid> SeedVolunteerAsync()
    {
        var id = VolunteerId.NewVolunteerId();
        var fio = Fio.Create("string", "string", "string").Value;
        var phone = Phone.Create("+7 (123) 123-12-21").Value;
        var email = Email.Create("email@email.com").Value;
        var description = Description.Create("description").Value;
        
        var volunteer = Volunteer
            .Create(id, fio, phone, email, description).Value;
        
        await WriteContext.Volunteers.AddAsync(volunteer);
        await WriteContext.SaveChangesAsync(CancellationToken.None);
        return volunteer.Id.Value;
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
        var speciesName = "test";
        
        var request = new CreateRequest(speciesName);
        
        var id = await Contract.AddSpecies(request, CancellationToken.None);


        return id.Value;
    }

    public async Task<Guid> SeedBreedAsync(Guid speciesId)
    {
        var breedName = Name.Create("Test").Value;
        
        var request = new AddBreedsRequest([breedName.Value]);
        
        var id = await Contract.AddBreeds(speciesId, request, CancellationToken.None);
        
        return id.Value.First();
    } 

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await Factory.ResetDatabaseAsync();
    }
}
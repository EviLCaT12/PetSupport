using System.Globalization;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.DataBase;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.SharedVO;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.ValueObjects.BreedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;
using PetFamily.Infrastructure.DbContexts;

namespace IntegrationTests.VolunteerTests;   

public class VolunteerBaseTest : IClassFixture<VolunteerTestsWebFactory>, IAsyncLifetime
{
    protected readonly Fixture Fixture;
    protected readonly WriteDbContext WriteContext;
    protected readonly IReadDbContext ReadContext;
    protected readonly IServiceScope Scope;
    protected readonly VolunteerTestsWebFactory Factory;
    
    protected VolunteerBaseTest(VolunteerTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        ReadContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        WriteContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        Fixture = new Fixture();
    }
    
    public async Task<Guid> SeedVolunteerAsync()
    {
        var id = VolunteerId.NewVolunteerId();
        var fio = VolunteerFio.Create("string", "string", "string").Value;
        var phone = Phone.Create("+7 (123) 123-12-21").Value;
        var email = Email.Create("email@email.com").Value;
        var description = Description.Create("description").Value;
        var exp = YearsOfExperience.Create(12).Value;
        IEnumerable<SocialWeb> socialWebsList = [];
        IEnumerable<TransferDetails> transferDetails = [];
        
        var volunteer = Volunteer
            .Create(id, fio, phone, email, description, exp, socialWebsList, transferDetails).Value;
        
        await WriteContext.Volunteers.AddAsync(volunteer, CancellationToken.None);
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
        IEnumerable<PetPhoto> photoList = [];
        
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
        var speciesName = Name.Create("Test").Value;
        
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
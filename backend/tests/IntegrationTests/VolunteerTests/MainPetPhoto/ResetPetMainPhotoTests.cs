using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.MainPetPhoto;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared.SharedVO;

namespace IntegrationTests.VolunteerTests.MainPetPhoto;

public class ResetPetMainPhotoTests : VolunteerBaseTest
{
    public ResetPetMainPhotoTests(VolunteerTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Reset_Flag_Main_Photo_From_Pet_Photo_Should_Be_Successful()
    {
        var volunteerId = await SeedVolunteerAsync();
        var volunteer = WriteContext.Volunteers
            .Include(v => v.AllOwnedPets)
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId = await SeedPetAsync(volunteer!);
        var pet = volunteer!.AllOwnedPets.FirstOrDefault(p => p.Id.Value == petId);
        
        var photo = PetPhoto.Create(FilePath.Create("photo.jpg", null).Value).Value;
        
        pet!.AddPhotos([photo]);
        pet.SetMainPhoto(photo);
        
        await WriteContext.SaveChangesAsync(CancellationToken.None);
        
        var command = new PetMainPhotoCommand(volunteerId, petId, photo.PathToStorage.Path);

        var sut = Scope.ServiceProvider
            .GetServices<ICommandHandler<PetMainPhotoCommand>>()
            .FirstOrDefault(s => s.GetType() == typeof(RemovePetMainPhotoHandler));
        
        var result = await sut!.HandleAsync(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        pet.GetPhotoByPath(photo.PathToStorage).Value.IsMain.Should().BeFalse();
    }
}
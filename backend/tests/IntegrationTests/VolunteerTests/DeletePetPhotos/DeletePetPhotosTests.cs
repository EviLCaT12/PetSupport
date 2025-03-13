using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.AddPetPhotos;
using PetFamily.Application.PetManagement.Commands.DeletePetPhotos;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared.SharedVO;

namespace IntegrationTests.VolunteerTests.DeletePetPhotos;

public class DeletePetPhotosTests : VolunteerBaseTest
{
    public DeletePetPhotosTests(VolunteerTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Delete_Pet_Photos_With_Work_Provider_Should_Be_Successful()
    {
        Factory.SetupSuccessRemoveFilesAsync();
                
        var volunteerId = await SeedVolunteerAsync();
        var volunteer = WriteContext.Volunteers
            .Include(v => v.AllOwnedPets)
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId = await SeedPetAsync(volunteer!);
        var pet = volunteer!.AllOwnedPets.FirstOrDefault(p => p.Id.Value == petId);
        
        var photoToDelete = PetPhoto.Create(FilePath.Create("photo.jpg", null).Value).Value;
        
        pet!.AddPhotos([photoToDelete]);
        await WriteContext.SaveChangesAsync(CancellationToken.None);
        
        var command = new DeletePetPhotosCommand(volunteerId, petId, [photoToDelete.PathToStorage.Path]);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<DeletePetPhotosCommand>>();
        
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        
        var petPhoto = WriteContext.Volunteers
            .Include(v => v.AllOwnedPets)
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId)!
            .AllOwnedPets.FirstOrDefault(p => p.Id.Value == petId)!
            .PhotoList;
        petPhoto.Should().BeEmpty();
    }
    
}
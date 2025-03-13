using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.AddPetPhotos;

namespace IntegrationTests.VolunteerTests.AddPetPhotosTests;

public class AddPetPhotosTest : VolunteerBaseTest
{
    public AddPetPhotosTest(VolunteerTestsWebFactory factory) : base(factory)
    {
        Fixture.Inject<Stream>(Stream.Null);
    }

    [Fact]
    public async Task Add_Photo_To_Pet_Should_Be_Successful()
    {
        //Arrange
        Factory.SetupSuccessUploadFilesAsync();
        
        var volunteerId = await SeedVolunteerAsync();
        var volunteer = WriteContext.Volunteers
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId = await SeedPetAsync(volunteer!);

        var command = Fixture.ToAddPetPhotosCommand(
            volunteerId,
            petId);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<AddPetPhotosCommand>>();
        
        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        var photo = ReadContext.Pets
            .ToList()
            .FirstOrDefault(p => p.Photos.Any());
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        photo.Should().NotBeNull();
    }

    [Fact]
    public async Task Add_Photo_To_Pet_Should_Be_Failed_While_Provider_Failes()
    {
        //Arrange
        Factory.SetupFailedUploadFilesAsync();
        
        var volunteerId = await SeedVolunteerAsync();
        var volunteer = WriteContext.Volunteers
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId = await SeedPetAsync(volunteer!);

        var command = Fixture.ToAddPetPhotosCommand(
            volunteerId,
            petId);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<AddPetPhotosCommand>>();
        
        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        var photo = ReadContext.Pets
            .ToList()
            .FirstOrDefault(p => p.Photos.Any());
        
        //Assert
        result.IsFailure.Should().BeTrue();
        photo.Should().BeNull();
    }
}
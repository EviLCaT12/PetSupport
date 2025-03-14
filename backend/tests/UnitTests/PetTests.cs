using FluentAssertions;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared.SharedVO;

namespace UnitTests;

public class PetTests
{
    [Fact]
    public void Delete_Should_MarkPetAsDeleted()
    {
        // Arrange
        var pet = CreatePet();

        // Act
        pet.Delete();

        // Assert
        pet.IsDeleted.Should().BeTrue();
    }
    
    [Fact]
    public void Restore_Should_MarkPetAsNotDeleted()
    {
        // Arrange
        var pet = CreatePet();
        pet.Delete();

        // Act
        pet.Restore();

        // Assert
        pet.IsDeleted.Should().BeFalse();
    }
    
    [Fact]
    public void SetPosition_Should_SetPosition_When_ValidDataProvided()
    {
        // Arrange
        var pet = CreatePet();
        var newPosition = Position.Create(5).Value;

        // Act
        pet.SetPosition(newPosition);

        // Assert
        pet.Position.Should().Be(newPosition);
    }
    
    [Fact]
    public void MoveForward_Should_MovePetForward_When_ValidDataProvided()
    {
        // Arrange
        var pet = CreatePet();
        pet.SetPosition(Position.Create(3).Value);

        // Act
        var result = pet.MoveForward(1, 5);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.Position.Value.Should().Be(4);
    }
    
    [Fact]
    public void MoveBackward_Should_MovePetBackward_When_ValidDataProvided()
    {
        // Arrange
        var pet = CreatePet();
        pet.SetPosition(Position.Create(3).Value);

        // Act
        var result = pet.MoveBackward(1, 5);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.Position.Value.Should().Be(2);
    }
    
    [Fact]
    public void AddPhotos_Should_AddPhotosToPet_When_ValidDataProvided()
    {
        // Arrange
        var pet = CreatePet();
        var photo1 = PetPhoto.Create(FilePath.Create("path1.jpg", null).Value).Value;
        var photo2 = PetPhoto.Create(FilePath.Create("path2.jpg", null).Value).Value;
        var photos = new List<PetPhoto> { photo1, photo2 };

        // Act
        pet.AddPhotos(photos);

        // Assert
        pet.PhotoList.Should().Contain(photos);
    }
    
    [Fact]
    public void DeletePhotos_Should_RemovePhotosFromPet_When_PhotosExist()
    {
        // Arrange
        var pet = CreatePet();
        var photo1 = PetPhoto.Create(FilePath.Create("path1.jpg", null).Value).Value;
        var photo2 = PetPhoto.Create(FilePath.Create("path2.jpg", null).Value).Value;
        var photos = new List<PetPhoto> { photo1, photo2 };

        pet.AddPhotos(photos);

        // Act
        var result = pet.DeletePhotos(new List<PetPhoto> { photo1 });

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.PhotoList.Should().NotContain(photo1);
        pet.PhotoList.Should().Contain(photo2);
    }
    
    [Fact]
    public void DeletePhotos_Should_ReturnError_When_PhotoNotFound()
    {
        // Arrange
        var pet = CreatePet();
        var photo1 = PetPhoto.Create(FilePath.Create("path1.jpg", null).Value).Value;
        var nonExistentPhoto = PetPhoto.Create(FilePath.Create("nonexistent.jpg", null).Value).Value;

        pet.AddPhotos(new List<PetPhoto> { photo1 });

        // Act
        var result = pet.DeletePhotos(new List<PetPhoto> { nonExistentPhoto });

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
    
    
    [Fact]
    public void SetMainPhoto_Should_ReturnError_When_MainPhotoAlreadySet()
    {
        // Arrange
        var pet = CreatePet();
        var photo1 = PetPhoto.Create(FilePath.Create("path1.jpg", null).Value).Value;
        var photo2 = PetPhoto.Create(FilePath.Create("path2.jpg", null).Value).Value;

        pet.AddPhotos(new List<PetPhoto> { photo1, photo2 });
        pet.SetMainPhoto(photo1);

        // Act
        var result = pet.SetMainPhoto(photo2);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
    
    [Fact]
    public void RemoveMainPhoto_Should_ReturnError_When_MainPhotoNotSet()
    {
        // Arrange
        var pet = CreatePet();
        var photo = PetPhoto.Create(FilePath.Create("path1.jpg", null).Value).Value;

        pet.AddPhotos(new List<PetPhoto> { photo });

        // Act
        var result = pet.RemoveMainPhoto(photo);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
    
    [Fact]
    public void GetPhotoByPath_Should_ReturnPhoto_When_PhotoExists()
    {
        // Arrange
        var pet = CreatePet();
        var photo = PetPhoto.Create(FilePath.Create("path1.jpg", null).Value).Value;

        pet.AddPhotos(new List<PetPhoto> { photo });

        // Act
        var result = pet.GetPhotoByPath(photo.PathToStorage);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(photo);
    }
    
    [Fact]
    public void GetPhotoByPath_Should_ReturnError_When_PhotoNotFound()
    {
        // Arrange
        var pet = CreatePet();
        var nonExistentPhotoPath = FilePath.Create("nonexistent.jpg", null).Value;

        // Act
        var result = pet.GetPhotoByPath(nonExistentPhotoPath);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
    
    [Fact]
    public void Update_Should_UpdatePetDetails_When_ValidDataProvided()
    {
        // Arrange
        var pet = CreatePet();
        var newName = Name.Create("Max").Value;
        var newDescription = Description.Create("Very friendly").Value;

        // Act
        pet.Update(
            newName,
            PetClassification.Create(Guid.NewGuid(), Guid.NewGuid()).Value,
            newDescription,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        // Assert
        pet.Name.Should().Be(newName);
        pet.Description.Should().Be(newDescription);
    }
    
    [Fact]
    public void ChangeHelpStatus_Should_ChangeHelpStatus_When_ValidDataProvided()
    {
        // Arrange
        var pet = CreatePet();
        var newHelpStatus = HelpStatus.SeekHome;

        // Act
        pet.ChangeHelpStatus(newHelpStatus);

        // Assert
        pet.HelpStatus.Should().Be(newHelpStatus);
    }
    
    private Pet CreatePet()
    {
        var id = PetId.NewPetId();
        var name = Name.Create("name").Value;
        var classification = PetClassification.Create(Guid.NewGuid(), Guid.NewGuid()).Value; 
        var description = Description.Create("description").Value;
        var color = Color.Create("color").Value;
        var healthInfo = HealthInfo.Create("health").Value;
        var address = Address.Create("string", "string", "string").Value;
        var dimensions = Dimensions.Create(1, 1).Value;
        var phone = Phone.Create("+7 (123) 123-12-21").Value;
        var isCastrate = true;
        var dateOfBirth = DateTime.Today;
        var isVaccinated = true;
        var helpStatus = "SeekHome";
        IEnumerable<TransferDetails> transferDetailsList = [];
        IEnumerable<PetPhoto> photoList = [];
        
        var pet = Pet.Create(
            id, name, classification, description, color, 
            healthInfo, address, dimensions, phone, isCastrate,
            dateOfBirth, isVaccinated, helpStatus, transferDetailsList, photoList).Value;
        
        return pet;
    }
}
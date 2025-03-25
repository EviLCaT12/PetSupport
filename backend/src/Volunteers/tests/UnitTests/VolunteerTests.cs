using FluentAssertions;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;
using Xunit.Abstractions;

namespace UnitTests;

public class VolunteerTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public VolunteerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Create_Volunteer_Should_Be_Successful()
    {
        //Act
        var result = CreateVolunteerWithoutPet();
        
        //Assert
        result.Should().NotBe(null);
    }

    [Fact]
    public void Update_Main_Info_Should_Be_Successful()
    {
        //Arrange
        var volunteer = CreateVolunteerWithoutPet();
        
        var newFio = Fio.Create("string1", "string1", "string1").Value;;
        var newPhone = Phone.Create("+7 (333) 123-12-21").Value;
        var newEmail = Email.Create("email@ema123il.com").Value;
        var newDescription = Description.Create("2222").Value;
        
        //Act
        volunteer.UpdateMainInfo(newFio, newPhone, newEmail, newDescription);
        
        //Assert
        volunteer.Fio.Should().BeEquivalentTo(newFio);
        volunteer.Phone.Should().BeEquivalentTo(newPhone);
        volunteer.Email.Should().BeEquivalentTo(newEmail);
        volunteer.Description.Should().BeEquivalentTo(newDescription);
    }
    
    [Fact]
    public void UpdatePet_Should_UpdatePetDetails_When_ValidDataProvided()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();

        volunteer.AddPet(pet);

        var newName = Name.Create("Max").Value;
        var newDescription = Description.Create("Very friendly").Value;

        // Act
        volunteer.UpdatePet(
            pet,
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
    public void HardDeletePet_Should_RemovePetFromList_When_PetExists()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();

        volunteer.AddPet(pet);

        // Act
        volunteer.HardDeletePet(pet);

        // Assert
        volunteer.AllOwnedPets.Should().NotContain(pet);
    }
    
    [Fact]
    public void SoftDeletePet_Should_MarkPetAsDeleted_When_PetExists()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();

        volunteer.AddPet(pet);

        // Act
        volunteer.SoftDeletePet(pet);

        // Assert
        pet.IsDeleted.Should().BeTrue();
    }
    
    [Fact]
    public void RestorePet_Should_RestorePet_When_PetExists()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();

        volunteer.AddPet(pet);
        volunteer.SoftDeletePet(pet);

        // Act
        var result = volunteer.RestorePet(pet.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.IsDeleted.Should().BeFalse();
    }
    
    [Fact]
    public void AddPet_Should_AddPetToList_When_ValidDataProvided()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();

        // Act
        var result = volunteer.AddPet(pet);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.AllOwnedPets.Should().Contain(pet);
    }
    
    [Fact]
    public void Soft_Delete_Volunteer_Should_Make_IsDeleted_True()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();

        // Act
        volunteer.Delete();

        // Assert
        volunteer.IsDeleted.Should().BeTrue();
    }
    
    [Fact]
    public void Soft_Delete_Volunteer_Should_Make_Added_Pet_Deletable()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet); 
        volunteer.AddPet(pet); 
        volunteer.AddPet(pet); 
        volunteer.AddPet(pet); 
        
        // Act
        volunteer.Delete();

        // Assert
        foreach (var pet1 in volunteer.AllOwnedPets)
        {
            pet1.IsDeleted.Should().BeTrue();
        }
    }
    
    [Fact]
    public void Restore_Volunteer_Should_Make_IsDeleted_False()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        volunteer.Delete();
        
        // Act
        volunteer.Restore();

        // Assert
        volunteer.IsDeleted.Should().BeFalse();
    }
    
    [Fact]
    public void Restore_Volunteer_Should_Make_Added_Pet_Retored()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet); 
        volunteer.AddPet(pet); 
        volunteer.AddPet(pet); 
        volunteer.AddPet(pet); 
        volunteer.Delete();
        
        // Act
        volunteer.Restore();

        // Assert
        foreach (var pet1 in volunteer.AllOwnedPets)
        {
            pet1.IsDeleted.Should().BeFalse();
        }
    }
    
    [Fact]
    public void AddPetPhotos_Should_AddPhotosToPet_When_ValidDataProvided()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet);

        var photo1 = Photo.Create(FilePath.Create("path1.jpg", null).Value).Value;
        var photo2 = Photo.Create(FilePath.Create("path2.jpg", null).Value).Value;
        var photos = new List<Photo> { photo1, photo2 };

        // Act
        volunteer.AddPetPhotos(pet.Id, photos);

        // Assert
        pet.PhotoList.Should().Contain(photos);
    }
    
    [Fact]
    public void DeletePetPhotos_Should_RemovePhotosFromPet_When_PhotosExist()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet);

        var photo1 = Photo.Create(FilePath.Create("path1.jpg", null).Value).Value;
        var photo2 = Photo.Create(FilePath.Create("path2.jpg", null).Value).Value;
        var photos = new List<Photo> { photo1, photo2 };

        volunteer.AddPetPhotos(pet.Id, photos);

        // Act
        var result = volunteer.DeletePetPhotos(pet.Id, new List<Photo> { photo1 });

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.PhotoList.Should().NotContain(photo1);
        pet.PhotoList.Should().Contain(photo2);
    }
    
    [Fact]
    public void DeletePetPhotos_Should_ReturnError_When_PhotoNotFound()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet);

        var photo1 = Photo.Create(FilePath.Create("path1.jpg", null).Value).Value;
        var photo2 = Photo.Create(FilePath.Create("path2.jpg", null).Value).Value;
        var photos = new List<Photo> { photo1 };

        volunteer.AddPetPhotos(pet.Id, photos);

        var nonExistentPhoto = Photo.Create(FilePath.Create("nonexistent.jpg", null).Value).Value;

        // Act
        var result = volunteer.DeletePetPhotos(pet.Id, new List<Photo> { nonExistentPhoto });

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
    
    [Fact]
    public void MovePetToSpecifiedPosition_Should_MovePetToNewPosition_When_ValidDataProvided()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet1 = CreatePet();
        var pet2 = CreatePet();
        var pet3 = CreatePet();

        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);
        volunteer.AddPet(pet3);

        var newPosition = Position.Create(1).Value;

        // Act
        var result = volunteer.MovePetToSpecifiedPosition(pet2.Id, newPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet2.Position.Value.Should().Be(1);
        pet1.Position.Value.Should().Be(2);
        pet3.Position.Value.Should().Be(3);
    }
    
    [Fact]
    public void MovePetToFirstPosition_Should_MovePetToFirstPosition_When_ValidDataProvided()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet1 = CreatePet();
        var pet2 = CreatePet();
        var pet3 = CreatePet();

        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);
        volunteer.AddPet(pet3);

        // Act
        var result = volunteer.MovePetToFirstPosition(pet3.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet3.Position.Value.Should().Be(1);
        pet1.Position.Value.Should().Be(2);
        pet2.Position.Value.Should().Be(3);
    }
    
    [Fact]
    public void MovePetToLastPosition_Should_MovePetToLastPosition_When_ValidDataProvided()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet1 = CreatePet();
        var pet2 = CreatePet();
        var pet3 = CreatePet();

        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);
        volunteer.AddPet(pet3);

        // Act
        var result = volunteer.MovePetToLastPosition(pet1.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet1.Position.Value.Should().Be(3);
        _testOutputHelper.WriteLine(pet1.Position.Value.ToString());
        pet2.Position.Value.Should().Be(1);
        _testOutputHelper.WriteLine(pet2.Position.Value.ToString());
        pet3.Position.Value.Should().Be(2);
        _testOutputHelper.WriteLine(pet3.Position.Value.ToString());
    }
    
    [Fact]
    public void MovePetToFirstPosition_Should_ReturnSuccess_When_PetIsAlreadyFirst()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet1 = CreatePet();
        var pet2 = CreatePet();

        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);

        // Act
        var result = volunteer.MovePetToFirstPosition(pet1.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet1.Position.Value.Should().Be(1);
        pet2.Position.Value.Should().Be(2);
    }
    
    [Fact]
    public void MovePetToLastPosition_Should_ReturnSuccess_When_PetIsAlreadyLast()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet1 = CreatePet();
        var pet2 = CreatePet();

        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);

        // Act
        var result = volunteer.MovePetToLastPosition(pet2.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet1.Position.Value.Should().Be(1);
        pet2.Position.Value.Should().Be(2);
    }
    
    [Fact]
    public void GetPetById_Should_ReturnPet_When_PetExists()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet);

        // Act
        var result = volunteer.GetPetById(pet.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(pet);
    }
    
    [Fact]
    public void GetPetById_Should_ReturnError_When_PetNotFound()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var nonExistentPetId = PetId.Create(Guid.NewGuid()).Value;

        // Act
        var result = volunteer.GetPetById(nonExistentPetId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
    
    [Fact]
    public void ChangePetHelpStatus_Should_ChangeStatus_When_ValidDataProvided()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet);

        var newHelpStatus = HelpStatus.SeekHome;

        // Act
        var result = volunteer.ChangePetHelpStatus(pet.Id, newHelpStatus);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.HelpStatus.Should().Be(newHelpStatus);
    }
    
    [Fact]
    public void ChangePetHelpStatus_Should_ReturnError_When_PetNotFound()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var nonExistentPetId = PetId.Create(Guid.NewGuid()).Value;
        var newHelpStatus = HelpStatus.SeekHome;

        // Act
        var result = volunteer.ChangePetHelpStatus(nonExistentPetId, newHelpStatus);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
    
    [Fact]
    public void SetPetMainPhoto_Should_SetMainPhoto_When_ValidDataProvided()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet);

        var filePath = FilePath.Create("path1.jpg", null).Value;
        var photo = Photo.Create(filePath).Value;

        // Act
        var result = volunteer.SetPetMainPhoto(pet, photo);
        photo = volunteer.GetPetPhoto(pet, photo.PathToStorage).Value;

        // Assert
        result.IsSuccess.Should().BeTrue();
        photo.IsMain.Should().BeTrue();
    }
    
    
    [Fact]
    public void RemovePetMainPhoto_Should_RemoveMainPhoto_When_ValidDataProvided()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet);

        var photo = Photo.Create(FilePath.Create("path1.jpg", null).Value).Value;
        pet.AddPhotos(new List<Photo> { photo });
        photo.SetMain();

        // Act
        var result = volunteer.RemovePetMainPhoto(pet, photo);

        // Assert
        result.IsSuccess.Should().BeTrue();
        photo.IsMain.Should().BeFalse();
    }
    
    [Fact]
    public void RemovePetMainPhoto_Should_ReturnError_When_PhotoIsNotMain()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet);

        var photo = Photo.Create(FilePath.Create("path1.jpg", null).Value);
        pet.AddPhotos(new List<Photo> { photo.Value });

        // Act
        var result = volunteer.RemovePetMainPhoto(pet, photo.Value);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
    
    [Fact]
    public void GetPetPhoto_Should_ReturnPhoto_When_PhotoExists()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet);

        var photo = Photo.Create(FilePath.Create("path1.jpg", null).Value);
        pet.AddPhotos(new List<Photo> { photo.Value });

        // Act
        var result = volunteer.GetPetPhoto(pet, photo.Value.PathToStorage);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(photo.Value);
    }
    
    [Fact]
    public void GetPetPhoto_Should_ReturnError_When_PhotoNotFound()
    {
        // Arrange
        var volunteer = CreateVolunteerWithoutPet();
        var pet = CreatePet();
        volunteer.AddPet(pet);

        var nonExistentPhotoPath = FilePath.Create("nonexistent.jpg", null).Value;

        // Act
        var result = volunteer.GetPetPhoto(pet, nonExistentPhotoPath);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }

    private Volunteer CreateVolunteerWithoutPet()
    {
        var id = VolunteerId.NewVolunteerId();
        var fio = Fio.Create("string", "string", "string").Value;
        var phone = Phone.Create("+7 (123) 123-12-21").Value;
        var email = Email.Create("email@email.com").Value;
        var description = Description.Create("description").Value;
        
        var volunteer = Volunteer
            .Create(id, fio, phone, email, description);
        
        return volunteer.Value;
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
        IEnumerable<Photo> photoList = [];
        
        var pet = Pet.Create(
            id, name, classification, description, color, 
            healthInfo, address, dimensions, phone, isCastrate,
            dateOfBirth, isVaccinated, helpStatus, transferDetailsList, photoList).Value;
        
        return pet;
    }
}
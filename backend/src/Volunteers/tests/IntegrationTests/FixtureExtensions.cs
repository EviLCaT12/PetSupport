using System.Globalization;
using AutoFixture;
using PetFamily.Core.Dto.Shared;
using PetFamily.Volunteers.Application.Commands.AddPet;
using PetFamily.Volunteers.Application.Commands.AddPetPhotos;
using PetFamily.Volunteers.Application.Commands.Create;
using PetFamily.Volunteers.Application.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Application.Commands.UpdatePet;
using PetFamily.Volunteers.Contracts.Dto.PetDto;
using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

namespace IntegrationTests;

public static class FixtureExtensions
{
    public static AddPetCommand ToAddPetCommand(
        this IFixture fixture,
        Guid volunteerId,
        Guid speciesId,
        Guid breedId)
    {
        var classification = new PetClassificationDto(speciesId, breedId);
        
        DateTime dateOfBirth = DateTime.Parse(
            "2025-03-12T13:13:14.384Z",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal);

        return fixture.Build<AddPetCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.Classification, classification)
            .With(c => c.OwnerPhone, "+7 (123) 123-12-21")
            .With(c => c.DateOfBirth, dateOfBirth)
            .With(c => c.HelpStatus, "SeekHome")
            .Create();
    }

    public static AddPetPhotosCommand ToAddPetPhotosCommand(
        this IFixture fixture,
        Guid volunteerId,
        Guid petId)
    {
        var stream = Stream.Null;
        IEnumerable<UploadPhotoDto> uploadPhotoDto = [new (stream, "test_photo.jpg")];

        return fixture.Build<AddPetPhotosCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.PetId, petId)
            .With(c => c.Photos, uploadPhotoDto)
            .Create();
    }

    public static CreateVolunteerCommand ToCreateVolunteerCommand(this IFixture fixture)
    {
        var fioDto = new FioDto("Test", "Test", "Test");
        var phone = "+7 (123) 123-12-21";
        var email = "test@test.com";

        return fixture.Build<CreateVolunteerCommand>()
            .With(c => c.Email, email)
            .With(c => c.PhoneNumber, phone)
            .With(c => c.Fio, fioDto)
            .Create();
    }
    
    public static UpdateVolunteerMainInfoCommand ToUpdateVolunteerMainInfoCommand(
        this IFixture fixture,
        Guid volunteerId,
        string newPhone)
    {
        var fioDto = new FioDto("Test", "Test", "Test");
        var email = "test@test.com";

        return fixture.Build<UpdateVolunteerMainInfoCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.Email, email)
            .With(c => c.Phone, newPhone)
            .With(c => c.Fio, fioDto)
            .Create();
    }
    
    public static UpdatePetCommand ToUpdatePetCommand(
        this IFixture fixture,
        Guid volunteerId,
        Guid petId,
        Guid speciesId,
        Guid breedId,
        string newPhone)
    {
        var classification = new PetClassificationDto(speciesId, breedId);
        
        DateTime dateOfBirth = DateTime.Parse(
            "2025-03-12T13:13:14.384Z",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal);

        return fixture.Build<UpdatePetCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.PetId, petId)
            .With(c => c.Classification, classification)
            .With(c => c.OwnerPhone, newPhone)
            .With(c => c.DateOfBirth, dateOfBirth)
            .Create();
    }
}
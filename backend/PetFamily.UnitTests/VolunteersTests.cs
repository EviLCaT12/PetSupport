using CSharpFunctionalExtensions;
using FluentAssertions;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.UnitTests;

public class VolunteersTests
{
    [Fact]
    public void Create_Volunteer_Return_Success()
    {
        var result = CreateVolunteer();
        
        //assert
        result.IsSuccess.Should().BeTrue();
    }

    private Result<Volunteer, ErrorList> CreateVolunteer()
    {
        var volunteerId = VolunteerId.NewVolunteerId();

        var fio = VolunteerFio.Create("string", "string", "string").Value;

        var phone = Phone.Create("+7 (123) 123-12-21").Value;
        
        var mail = Email.Create("test@test.com").Value;
        
        var description = Description.Create("description").Value;
        
        var exp = YearsOfExperience.Create(1).Value;

        List<SocialWeb> socialWebs = [];
        
        List<TransferDetails> transferDetails = [];

        var result = Volunteer.Create(
            volunteerId,
            fio,
            phone,
            mail,
            description,
            exp,
            socialWebs,
            transferDetails
        );

        if (result.IsFailure)
            return new ErrorList([result.Error]);

        return result.Value;
    }
}
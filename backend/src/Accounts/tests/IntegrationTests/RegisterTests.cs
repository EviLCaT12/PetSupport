using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.VolunteerDto;

namespace IntegrationTests;

public class RegisterTests : AccountBaseTests
{
    public RegisterTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Register_User_should_be_successful()
    {
        //Arrange
        var email = Guid.NewGuid() + "@test.com";
        var name = Guid.NewGuid() + ".";
        var password = "A" + Guid.NewGuid() ;
        var fioDto = new FioDto("string", "string", "string");
        
        
        var command = new RegisterUserCommand(email, name, fioDto, password);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<RegisterUserCommand>>();
        
        // Factory.SetupSuccessEmailSearch();
        // Factory.SetupSuccessCreateUser();
        
        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        var isUserExist = WriteContext.Users
            .ToList()
            .First();
        isUserExist.Should().NotBeNull();
    }
}
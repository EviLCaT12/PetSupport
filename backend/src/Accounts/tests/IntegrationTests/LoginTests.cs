using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Commands.LoginUser;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.VolunteerDto;

namespace IntegrationTests;

public class LoginTests : AccountBaseTests
{
    public LoginTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Login_User_Should_Be_Successful()
    {
        //Arrange
        var email = Guid.NewGuid() + "@test.com";
        var name = Guid.NewGuid() + ".";
        var password = "A" + Guid.NewGuid() ;
        var fioDto = new FioDto("string", "string", "string");
        
        var commandForRegister = new RegisterUserCommand(email, name, fioDto ,password);
        var register = Scope.ServiceProvider.GetRequiredService<ICommandHandler<RegisterUserCommand>>();
        await register.HandleAsync(commandForRegister, CancellationToken.None);
        
        var commandForLogin = new LoginUserCommand(email, password);
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<string, LoginUserCommand>>();
        
        //Act
        var result = await sut.HandleAsync(commandForLogin, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Should().NotBeNull();
        
    }
}
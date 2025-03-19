using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Commands.LoginUser;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Core.Abstractions;

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
        
        var commandForRegister = new RegisterUserCommand(email, name, password);
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
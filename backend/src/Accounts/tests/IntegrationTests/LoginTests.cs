using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Commands.LoginUser;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;
using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

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

        var scope1 = Factory.Services.CreateScope();
        var scope2 = Factory.Services.CreateScope();
        
        var commandForRegister = new RegisterUserCommand(email, name, fioDto ,password);
        var register = scope1.ServiceProvider.GetRequiredService<ICommandHandler<RegisterUserCommand>>();
        await register.HandleAsync(commandForRegister, CancellationToken.None);
        
        var commandForLogin = new LoginUserCommand(email, password);
        var sut = scope2.ServiceProvider.GetRequiredService<ICommandHandler<LoginResponse, LoginUserCommand>>();
        
        //Act
        var result = await sut.HandleAsync(commandForLogin, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Should().NotBeNull();
        
    }
}
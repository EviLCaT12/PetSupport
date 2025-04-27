using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Commands.EnrollVolunteer;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Accounts.Domain.Entities.AccountEntitites;
using PetFamily.Accounts.Infrastructure.Contexts;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;
using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

namespace IntegrationTests;

public class EnrollVolunteerTests : AccountBaseTests
{
    public EnrollVolunteerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Enroll_Volunteer_Should_Be_Success()
    {
        //Arrange
        var email = Guid.NewGuid() + "@test.com";
        var name = Guid.NewGuid() + ".";
        var password = "A" + Guid.NewGuid() ;
        var fioDto = new FioDto("string", "string", "string");
        var description = "string";
        var exp = 0;
        var phone = "+7 (123) 123-12-12";
        
        using var scope1 = Factory.Services.CreateScope();
        using var scope2 = Factory.Services.CreateScope();
        
        var commandForRegister = new RegisterUserCommand(email, name, fioDto ,password);
        var register = scope1.ServiceProvider.GetRequiredService<ICommandHandler<RegisterUserCommand>>();
        await register.HandleAsync(commandForRegister, CancellationToken.None);
        var context = scope1.ServiceProvider.GetRequiredService<WriteAccountsDbContext>();
        var userId = await context.Users.SingleOrDefaultAsync(x => x.Email == email);
        
        
        var commandForEnroll = new EnrollVolunteerCommand(
            userId!.Id, 
            exp,
            phone,
            description);
        var sut = scope2.ServiceProvider.GetRequiredService<ICommandHandler<Guid, EnrollVolunteerCommand>>();

        //Act
        var result = await sut.HandleAsync(commandForEnroll, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        
        var userCount = WriteContext.Users.Where(u => u.Email == email);
        userCount.Count().Should().Be(1);
        
        var user = await WriteContext.Users
            .Include(user => user.Roles)
            .FirstAsync(u => u.Id == userId.Id);
        
        user.Roles[0].Name.Should().Be(VolunteerAccount.Volunteer);
        
        var participantAccount = WriteContext.ParticipantAccounts;
        participantAccount.Should().BeEmpty();
        
        var volunteerAccount = WriteContext.VolunteerAccounts.First();
        volunteerAccount.Should().NotBeNull();
    }
}
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto.Shared;
using PetFamily.Application.PetManagement.Commands.UpdateSocialWeb;

namespace IntegrationTests.VolunteerTests.UpdateSocialWebs;

public class UpdateSocialWebsTests : VolunteerBaseTest
{
    public UpdateSocialWebsTests(VolunteerTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Update_SocialWebs_Should_Be_SuccessFul()
    {
        var volunteerId = await SeedVolunteerAsync();
        var newSocialWeb = new SocialWebDto("test", "test2");
        
        var command = new UpdateVolunteerSocialWebCommand(volunteerId, [newSocialWeb]);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateVolunteerSocialWebCommand>>();
        
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteerId);

        var socialWeb = ReadContext.Volunteers
            .FirstOrDefault(v => v.Id == volunteerId)!
            .SocialWebs.First();
        socialWeb.Name.Should().Be(newSocialWeb.Name);
    }
}
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;
using PetFamily.Volunteers.Application.Commands.UpdateSocialWeb;

namespace IntegrationTests.UpdateSocialWebs;

public class UpdateSocialWebsTests : VolunteerBaseTest
{
    public UpdateSocialWebsTests(TestsWebFactory factory) : base(factory)
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
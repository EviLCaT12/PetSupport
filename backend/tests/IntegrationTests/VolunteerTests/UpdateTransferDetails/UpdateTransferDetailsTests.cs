using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto.Shared;
using PetFamily.Application.PetManagement.Commands.UpdateSocialWeb;
using PetFamily.Application.PetManagement.Commands.UpdateTransferDetails;

namespace IntegrationTests.VolunteerTests.UpdateTransferDetails;

public class UpdateTransferDetailsTests : VolunteerBaseTest
{
    public UpdateTransferDetailsTests(VolunteerTestsWebFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Update_Transfer_Details_Should_Be_SuccessFul()
    {
        var volunteerId = await SeedVolunteerAsync();
        var newTransferDetails = new TransferDetailDto("test", "test2");
        
        var command = new UpdateVolunteerTransferDetailsCommand(volunteerId, [newTransferDetails]);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateVolunteerTransferDetailsCommand>>();
        
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteerId);

        var transferDetails = ReadContext.Volunteers
            .FirstOrDefault(v => v.Id == volunteerId)!
            .TransferDetails.First();
        transferDetails.Name.Should().Be(newTransferDetails.Name);
    }
}
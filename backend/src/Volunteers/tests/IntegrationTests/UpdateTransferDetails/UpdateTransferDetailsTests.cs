using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;
using PetFamily.Volunteers.Application.Commands.UpdateTransferDetails;

namespace IntegrationTests.UpdateTransferDetails;

public class UpdateTransferDetailsTests : VolunteerBaseTest
{
    public UpdateTransferDetailsTests(TestsWebFactory factory) : base(factory)
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
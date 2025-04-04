using FluentAssertions;
using PetFamily.VolunteerRequest.Domain.Enums;

namespace UnitTests;

public class VolunteerRequestTests
{
    [Fact]
    public void Take_Request_On_Review_Should_Be_Success()
    {
        //Arrange
        var request = Utilities.CreateVolunteerRequestWithoutRejectionComment(1).First();
        
        //Act
        var result = request.TakeRequestOnReview();

        //Assert
        result.Status.Should().Be(Status.OnReview);
    }

    [Fact]
    public void Send_For_Revision_With_Valid_Comment_Should_Be_Success()
    {
        //Arrange
        var request = Utilities.CreateVolunteerRequestWithoutRejectionComment(1).First();

        var comment = Utilities.CreateValidRejectionComment(null, null, null);

        //Act
        var result = request.SendForRevision(comment);

        //Assert
        result.Status.Should().Be(Status.RevisionRequired);
        result.RejectionComment.Should().NotBeNull();
    }
    
    [Fact]
    public void Reject_Request_With_Valid_Comment_Should_Be_Success()
    {
        //Arrange
        var request = Utilities.CreateVolunteerRequestWithoutRejectionComment(1).First();

        var comment = Utilities.CreateValidRejectionComment(null, null, null);

        //Act
        var result = request.RejectRequest(comment);

        //Assert
        result.Status.Should().Be(Status.Rejected);
        result.RejectionComment.Should().NotBeNull();
    }
    
    [Fact]
    public void Approve_Request_Should_Be_Success()
    {
        //Arrange
        var request = Utilities.CreateVolunteerRequestWithoutRejectionComment(1).First();
        
        //Act
        var result = request.ApproveRequest();

        //Assert
        result.Status.Should().Be(Status.Approved);
    }
}
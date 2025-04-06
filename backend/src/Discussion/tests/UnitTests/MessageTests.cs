namespace UnitTests;

public class MessageTests
{
    [Fact]
    public void Edit_Message_Should_Be_Success()
    {
        //Arrange
        var message = Utilities.CreateValidMessage();
        
        var newText = Guid.NewGuid().ToString();
        
        //Act
    }
    
}
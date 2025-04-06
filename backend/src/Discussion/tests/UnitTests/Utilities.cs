using PetFamily.Discussion.Domain.Entities;
using PetFamily.Discussion.Domain.ValueObjects;

namespace UnitTests;

public class Utilities
{
    public static Message CreateValidMessage()
    {
        var text = "This is a test message";
        
        var message = new Message(
            MessageId.NewId(),
            Guid.NewGuid(),
            Text.Create(text).Value);
        
        return message;
    }
}
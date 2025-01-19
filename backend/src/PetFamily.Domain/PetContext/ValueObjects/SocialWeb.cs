using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetContext.ValueObjects;

public record SocialWeb
{
    public string Link { get; private set; } 
    
    public string Name { get; private set; }

    private SocialWeb(string link, string name)
    {
        Link = link;
        Name = name;
    }

    public static Result<SocialWeb> Create(string link, string name)
    {
        if (string.IsNullOrWhiteSpace(link))
            return Result.Failure<SocialWeb>("Link cannot be null or empty");
        
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<SocialWeb>("Name cannot be null or empty");
        
        var socialWeb = new SocialWeb(link, name);
        
        return Result.Success(socialWeb);
    }
    
}
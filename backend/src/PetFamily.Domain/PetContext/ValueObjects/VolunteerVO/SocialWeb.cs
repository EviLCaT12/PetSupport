using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

public record SocialWeb
{
    public string Link { get; private set; } 
    
    public string Name { get; private set; }

    private SocialWeb(string link, string name)
    {
        Link = link;
        Name = name;
    }

    public static Result<SocialWeb, Error> Create(string link, string name)
    {
        if (string.IsNullOrWhiteSpace(link))
            return ErrorList.General.ValueIsRequired(nameof(Link));
        
        if (string.IsNullOrWhiteSpace(name))
            return ErrorList.General.ValueIsRequired(nameof(Name));
        
        var validSocialWeb = new SocialWeb(link, name);

        return validSocialWeb;
    }
    
}
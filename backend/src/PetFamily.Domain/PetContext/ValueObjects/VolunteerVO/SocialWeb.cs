using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

public record SocialWeb
{
    public SocialWeb() {} //Если оставить его private, то получаю ошибку от efcore Microsoft.EntityFrameworkCore.Query[10100]
    private SocialWeb(string link, string name)
    {
        Link = link;
        Name = name;
    }
    public string Link { get; private set; } 
    
    public string Name { get; private set; }



    public static Result<SocialWeb, Error> Create(string link, string name)
    {
        if (string.IsNullOrWhiteSpace(link))
            return Errors.General.ValueIsRequired(nameof(Link));
        
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired(nameof(Name));
        
        var validSocialWeb = new SocialWeb(link, name);

        return validSocialWeb;
    }
    
}
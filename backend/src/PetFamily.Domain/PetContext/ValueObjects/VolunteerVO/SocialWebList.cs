using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

public record SocialWebList
{
    private readonly List<SocialWeb> _socialWebs;
    
    public IReadOnlyList<SocialWeb> SocialWebs => _socialWebs;

    private SocialWebList(List<SocialWeb> socialWebs)
    {
        _socialWebs = socialWebs;
    }

    //ef core
    private SocialWebList() { }
    
    public static Result<SocialWebList, Error> Create(List<SocialWeb> socialWebs) =>
        new SocialWebList(socialWebs);
}
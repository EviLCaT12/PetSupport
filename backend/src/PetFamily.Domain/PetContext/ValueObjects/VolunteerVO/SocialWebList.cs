using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

public record SocialWebList
{
    private readonly List<SocialWeb> _socialWebs;
    
    public IReadOnlyList<SocialWeb> SocialWebs => _socialWebs;

    private SocialWebList(IEnumerable<SocialWeb> socialWebs)
    {
        _socialWebs = socialWebs.ToList();
    }

    //ef core
    private SocialWebList() { }
    
    public static Result<SocialWebList, Error> Create(List<SocialWeb> socialWebs) =>
        new SocialWebList(socialWebs);
}
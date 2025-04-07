using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.VolunteerRequest.Domain.ValueObjects;

public class VolunteerInfo : ValueObject
{
    private VolunteerInfo() { }

    public VolunteerInfo(
        Fio fullName,
        Description description,
        Email email,
        YearsOfExperience experience)
    {
        FullName = fullName;
        Description = description;
        Email = email;
        Experience = experience;
    }
    
    public Fio FullName { get; }
    
    public Description Description { get; }
    
    public Email Email { get; }
    
    public YearsOfExperience Experience { get; }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Email;
        
        yield return FullName;
        
        yield return Description;
        
    }
}
using System.Collections;

namespace PetFamily.Domain.Shared.Error;

public class ErrorList(IEnumerable<Error> errors) : IEnumerable<Error>
{
    private readonly List<Error> _errors = [..errors];

    public IEnumerator<Error> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
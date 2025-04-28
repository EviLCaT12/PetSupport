using System.Collections;

namespace FilesService.Error.Models;

public class ErrorList(IEnumerable<Error> errors) : IEnumerable<Error>
{
    private readonly List<Error> _errors = [..errors];
    public IEnumerator<Error> GetEnumerator()
    {
        return errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
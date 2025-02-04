using PetFamily.Domain.Shared.Error;

namespace PetFamily.API.Response;

public record Envelop
{
    public object? Result { get;}
    public ErrorList? Errors { get;}
    public DateTime TimeGenerated { get;}

    private Envelop(object? result, ErrorList? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }

    public static Envelop Ok(object? result = null) => new(result, null);
    
    public static Envelop Error(ErrorList errors) => new(null, errors);

}
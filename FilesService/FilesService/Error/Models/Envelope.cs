namespace FilesService.Error.Models;

public class Envelope
{
    public object? Result { get; }
    
    public ErrorList? ErrorList { get; }
    
    public DateTime TimeGenerated { get; }

    private Envelope(object? result, ErrorList? errorList)
    {
        Result = result;
        ErrorList = errorList;
        TimeGenerated = DateTime.Now;
    }

    public Envelope Ok(object? result = null) => new (result, null);
    
    public Envelope Fail(ErrorList errors) => new (null, errors);
}
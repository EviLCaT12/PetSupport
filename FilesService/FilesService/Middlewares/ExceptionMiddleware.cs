using FilesService.Core.Models;

namespace FilesService.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            
            var error = Error.Failure("server.error", e.Message);
            
            var envelope = Envelope.Fail(error.ToErrorList());
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}